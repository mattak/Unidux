namespace Unidux.Example.List
{
    public static class List
    {
        public enum ActionType
        {
            Add
        }

        public class Action
        {
            public ActionType ActionType;
            public string Text;
        }

        public static State Reducer(State state, Action action)
        {
            switch (action.ActionType)
            {
                case ActionType.Add:
                    state.List.Texts.Add(action.Text);
                    state.List.SetStateChanged();
                    break;
            }

            return state;
        }

        public static class ActionCreator
        {
            public static Action Add(string text)
            {
                return new Action() {ActionType = ActionType.Add, Text = text};
            }
        }
    }
}