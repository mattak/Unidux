namespace Unidux.Example.List
{
    public static class ListReducer
    {
        public static State Reduce(State state, ListAddAction action)
        {
            state.Texts.Add(action.Text);
            return state;
        }
    }
}
