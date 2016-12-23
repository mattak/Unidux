namespace Unidux.Example.List
{
    public sealed partial class Unidux
    {
        partial void AddReducers(Store<State> store)
        {
            store.AddReducer<List.Action>(List.Reducer);
        }
    }
}
