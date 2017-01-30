# <a href="https://github.com/mattak/Unidux"><img src="https://raw.githubusercontent.com/mattak/Unidux/master/art/unidux-logo-horizontal.png" height="60"></a>

[![Join the chat at https://gitter.im/Unidux/Lobby](https://badges.gitter.im/Join%20Chat.svg)](https://gitter.im/Unidux/Lobby?utm_source=badge&utm_medium=badge&utm_campaign=pr-badge&utm_content=badge)

Unidux is practical application architecture for Unity3D UI.

It's inspired by Redux.

# Install

Import unitypackage from [latest releases](https://github.com/mattak/Unidux/releases).

# Usage

1) Create your Unidux singleton and place it to unity scene.

```csharp
public sealed partial class Unidux : SingletonMonoBehaviour<Unidux>
{
    partial void AddReducers(Store<State> store);

    private ReplaySubject<State> _subject;
    private Store<State> _store;
    private State _state;

    public static State State
    {
        get { return Instance._state = Instance._state ?? new State(); }
    }

    public static ReplaySubject<State> Subject
    {
        get { return Instance._subject = Instance._subject ?? new ReplaySubject<State>(); }
    }

    public static Store<State> Store
    {
        get
        {
            if (Instance._store == null)
            {
                Instance._store = new Store<State>(State);
                Instance._store.AddRenderer(state => Subject.OnNext(state));
                Instance.AddReducers(Instance._store);
            }
            return Instance._store;
        }
    }

    void Update()
    {
        Store.Update();
    }
}

public sealed partial class Unidux
{
    partial void AddReducers(Store<State> store)
    {
        store.AddReducer<Count.ActionType>(Count.Reducer);
    }
}
```

2) Create state class to store application state.

```csharp
using Unidux;
public class State : StateBase<State>
{
    public int Count { get; set; }
}
```

3) Define action to change state. Define Reducer to move state.

```csharp
public static class Count
{
    public enum ActionType
    {
        Increment,
        Decrement
    }

    public class Action
    {
        public ActionType ActionType;
    }

    public static State Reducer(State state, ActionType action)
    {
        switch (action)
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

    public static class ActionCreator
    {
        public static Action Increment()
        {
            return new Action() {ActionType = ActionType.Increment};
        }

        public static Action Decrement()
        {
            return new Action() {ActionType = ActionType.Decrement};
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
    public Count.ActionType ActionType = Count.ActionType.Increment;

    void Start()
    {
        this.GetComponent<Button>()
            .OnClickAsObservable()
            .Subscribe(state => Unidux.Store.Dispatch(ActionType))
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

# Dependencies

- [UniRx](https://github.com/neuecc/UniRx) is refered on Example

# Thanks

- [@austinmao](https://github.com/austinmao) for suggestion of Ducks and UniRx.
- [@pine](https://github.com/pine) for description improvement.

# License

[MIT](./LICENSE.md)
