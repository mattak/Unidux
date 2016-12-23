namespace Unidux.Example.Counter
{
    public sealed partial class Unidux
    {
        partial void AddReducers(Store<State> store)
        {
            store.AddReducer<Count.ActionType>(Count.Reducer);
        }
    }
}