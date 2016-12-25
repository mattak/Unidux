namespace Unidux.Example.Todo
{
    public sealed partial class Unidux
    {
        partial void AddReducers(Store<State> store)
        {
            store.AddReducer<TodoDuck.Action>(TodoDuck.Reducer);
            store.AddReducer<TodoVisibilityDuck.Action>(TodoVisibilityDuck.Reducer);
        }
    }
}