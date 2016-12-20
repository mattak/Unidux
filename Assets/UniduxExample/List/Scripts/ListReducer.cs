namespace Unidux.Example.List
{
    // Action
    public class ListAddAction
    {
        public string Text;
    }

    public static class ListReducer
    {
        // Reducer
        public static State Reducer(State state, ListAddAction action)
        {
            state.List.Texts.Add(action.Text);
            state.List.SetStateChanged();
            return state;
        }

        // ActionCreator
        public static ListAddAction AddList(string text)
        {
            return new ListAddAction() {Text = text};
        }
    }
}