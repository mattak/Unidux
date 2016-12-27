namespace Unidux.Example.Todo
{
    public static class TodoVisibilityDuck
    {
        public enum ActionType
        {
            SET_VISIBILITY,
        }

        public class Action
        {
            public ActionType ActionType;
            public VisibilityFilter Filter;
        }

        public static class ActionCreator
        {
            public static Action SetVisibility(VisibilityFilter filter)
            {
                return new Action()
                {
                    ActionType = ActionType.SET_VISIBILITY,
                    Filter = filter,
                };
            }
        }

        public static State Reducer(State state, Action action)
        {
            switch (action.ActionType)
            {
                case ActionType.SET_VISIBILITY:
                    state.Todo = SetVisibility(state.Todo, action.Filter);
                    return state;
            }

            return state;
        }

        public static TodoState SetVisibility(TodoState state, VisibilityFilter filter)
        {
            state.Filter = filter;
            state.SetStateChanged();
            return state;
        }
    }
}