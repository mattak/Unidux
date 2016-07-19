# <a href="https://github.com/mattak/Unidux"><img src="https://raw.githubusercontent.com/mattak/Unidux/master/art/unidux-logo-horizontal.png" height="60"></a>

Unidux is practical application architecture for Unity3D UI.

It's inspired by Redux.

# Install

Import unitypackage from [latest releases](https://github.com/mattak/Unidux/releases).

# Usage

1) Create your Unidux singleton and place it to unity scene.

```
using Unidux;
public class Unidux : SingletonMonoBehaviour<Unidux>
{
    private Store<State> _store;
    public Store<State> Store
    {
        get
        {
            if (null == _store)
            {
                _store = new Store<State>(new State());
                // Add reducers in this place.
                _store.AddReducer<CountAction>(CountReducer.Reduce);
            }
            return _store;
        }
    }

    void Update()
    {
        this.Store.Update();
    }
}
```

2) Create state class to store application state.

```
using Unidux;
public class State : StateBase<State>
{
    public int Count { get; set; }
}
```

3) Define action to change state.

```
public enum CountAction
{
    Increment,
    Decrement
}
```

4) Create reducer to update state

```
using Unidux;

public static class CountReducer
{
    public static State Reduce(State state, CountAction action)
    {
        switch (action)
        {
            case CountAction.Increment:
                state.Count++;
                break;
            case CountAction.Decrement:
                state.Count--;
                break;
        }

        return state;
    }
}
```

5) Create Renderer to display state and attach it to Text GameObject.

```
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class CountRenderer : MonoBehaviour
{
    void OnEnable()
    {
        var text = this.GetComponent<Text>();
        var store = Unidux.Instance.Store;
        this.AddDisableTo(store, state => text.text = state.Count.ToString());
    }
}
```

6) Create dispatcher to update count and attach it to GameObject.

```
[RequireComponent(typeof(Button))]
public class CountDispatcher : MonoBehaviour
{
    public CountAction ActionType = CountAction.Increment;

    void Start()
    {
        var button = this.GetComponent<Button>();
        button.onClick.AddListener(() => Unidux.Instance.Store.Dispatch(ActionType));
    }
}
```

That's it!


# Example

- [Counter](Assets/UniduxExample/Counter)
- [List](Assets/UniduxExample/List)

# License

[MIT](./LICENSE.md)
