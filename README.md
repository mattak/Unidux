# <a href="https://github.com/mattak/Unidux"><img src="https://raw.githubusercontent.com/mattak/Unidux/master/art/unidux-logo-horizontal.png" height="60"></a>

[![Join the chat at https://gitter.im/Unidux/Lobby](https://badges.gitter.im/Join%20Chat.svg)](https://gitter.im/Unidux/Lobby?utm_source=badge&utm_medium=badge&utm_campaign=pr-badge&utm_content=badge)

Unidux is practical application architecture for Unity3D UI.

It's inspired by Redux.

# Install 
Import unitypackage from [latest releases](https://github.com/mattak/Unidux/releases).

# Usage

1) Create your Unidux singleton and place it to unity scene.

```csharp
using UniRx;
using Unidux;

public sealed class Unidux : SingletonMonoBehaviour<Unidux>, IStoreAccessor
{
    public TextAsset InitialStateJson;

    private Store<State> _store;

    public IStoreObject StoreObject
    {
        get { return Store; }
    }

    public static State State
    {
        get { return Store.State; }
    }

    public static Subject<State> Subject
    {
        get { return Store.Subject; }
    }

    private static State InitialState
    {
        get
        {
            return Instance.InitialStateJson != null
                ? JsonUtility.FromJson<State>(Instance.InitialStateJson.text)
                : new State();
        }
    }

    public static Store<State> Store
    {
        get { return Instance._store = Instance._store ?? new Store<State>(InitialState, new Count.Reducer()); }
    }

    public static object Dispatch<TAction>(TAction action)
    {
        return Store.Dispatch(action);
    }

    void Update()
    {
        Store.Update();
    }
}
```

_Note: `ReplaySubject` is a [ReactiveX concept](http://reactivex.io/documentation/subject.html)
provided by [UniRx](https://github.com/neuecc/UniRx) in this example._

2) Create state class to store application state.

```csharp
using System;

[Serializable]
public class State : StateBase
{
    public int Count = 0;
}
```

3) Define action to change state. Define Reducer to move state.

```csharp
public static class Count
{
    // specify the possible types of actions
    public enum ActionType
    {
        Increment,
        Decrement
    }

    // actions must have a type and may include a payload
    public class Action
    {
        public ActionType ActionType;
    }

    // ActionCreators creates actions and deliver payloads
    // in redux, you do not dispatch from the ActionCreator to allow for easy testability
    public static class ActionCreator
    {
        public static Action Create(ActionType type)
        {
            return new Action() {ActionType = type};
        }

        public static Action Increment()
        {
            return new Action() {ActionType = ActionType.Increment};
        }

        public static Action Decrement()
        {
            return new Action() {ActionType = ActionType.Decrement};
        }
    }

    // reducers handle state changes
    public class Reducer : ReducerBase<State, Action>
    {
        public override State Reduce(State state, Action action)
        {
            switch (action.ActionType)
            {
                case ActionType.Increment:
                    state.Count++;
                    break;
                case ActionType.Decrement:
                    state.Count--;
                    break;
            }

            return state;
        }
    }
}
```

4) Create Renderer to display state and attach it to Text GameObject.

```csharp
[RequireComponent(typeof(Text))]
public class CountRenderer : MonoBehaviour
{
    void OnEnable()
    {
        var text = this.GetComponent<Text>();

        Unidux.Subject
            .TakeUntilDisable(this)
            .StartWith(Unidux.State)
            .Subscribe(state => text.text = state.Count.ToString())
            .AddTo(this)
            ;
    }
}
```

5) Create dispatcher to update count and attach it to GameObject.

```csharp
[RequireComponent(typeof(Button))]
public class CountDispatcher : MonoBehaviour
{
    public Count.Action Action = Count.ActionCreator.Increment();

    void Start()
    {
        this.GetComponent<Button>()
            .OnClickAsObservable()
            .Subscribe(state => Unidux.Store.Dispatch(Action))
            .AddTo(this)
            ;
    }
}
```

That's it!


# Example

- [Counter](Assets/Plugins/Unidux/Examples/Counter)
- [List](Assets/Plugins/Unidux/Examples/List)
- [Todo](Assets/Plugins/Unidux/Examples/Todo)
- [Middlewares](Assets/Plugins/Unidux/Examples/Middlewares)
- [SimpleHttp](Assets/Plugins/Unidux/Examples/SimpleHttp)

# Dependencies

- [UniRx](https://github.com/neuecc/UniRx)
- [MiniJSON](https://gist.github.com/darktable/1411710) (for Unidux.Experimental.Editor.StateJsonEditor)

# API

## `StateBase`

```csharp
public class State : StateBase
{
    public int Count;
}
```

### `<StateBase>.Clone()`

```csharp
State _state = new State();
State _clonedState = _state.Clone();
```

Create a deep clone of the current state. Useful for Immutability.

## `Store`

```csharp
IReducer[] reducers = new IReducer[]{};
Store _store = new Store<State>(State, reducers);
// State must extend StateBase
```

### `<Store>.State`

Get the state as passed to the constructor.

### `<Store>.Dispatch(object)`

Dispatch an event of `TAction` object,
which will trigger a `Reducer<TAction>`.

### `<Store>.ApplyMiddlewares(params Middleware[] middlewares)`

Apply middlewares to Store object
which implement delegate function of [Middleware](Assets/Plugins/Unidux/Scripts/IMiddleware.cs#L5).

### `<Store>.Update()`

When at least one reducer has been executed,
trigger all the renderers with a copy of the current state.

### `<Store>.ForceUpdate()`

Trigger all registered renderers with a copy of the current state
regardless of any reducers having been executed.

## `SingletonMonoBehaviour`

```csharp
public class Foo : SingletonMonoBehaviour<Foo> {}
```

A singleton base class to extend.
Extends `MonoBehaviour`.

### `<SingletonMonoBehaviour>.Instance`

```csharp
public class Foo : SingletonMonoBehaviour<Foo> {}

Foo.Instance
```

The instance of the base class.

# Performance

## `Clone()`

Default implemention of `StateBase.Clone()` is not fast, because it uses `BinaryFormatter` & `MemoryStream`.
And Unidux creates new State on every State chaning (it affects a few milliseconds).
So in case of requiring performance, override clone method with your own logic.

e.g.

```csharp
[Serializable]
class State : StateBase
{
    public override object Clone()
    {
        // implement your custom deep clone code
    }
}
```

## `Equals()`

Default implemention of `StateBase.Equals()` and `StateElement.Equals()` is not fast, because it uses fields and properties reflection.
In case of edit state on UniduxPanel's StateEditor, it calls `Equals()` in order to set `IsStateChanged` flags automatically.
So in case of requiring performance, override `Equals()` method with your own logic.

e.g.

```csharp
[Serializable]
class State : StateBase
{
    public override bool Equals(object obj)
    {
        // implement your custom equality check code
    }
}
```

# Thanks

- [@austinmao](https://github.com/austinmao) for suggestion of Ducks and UniRx.
- [@pine](https://github.com/pine) for description improvement.
- [@jesstelford](https://github.com/jesstelford) for fix document.
- [@tenmihi](https://github.com/tenmihi) for fix document.

# License

[MIT](./LICENSE.md)
