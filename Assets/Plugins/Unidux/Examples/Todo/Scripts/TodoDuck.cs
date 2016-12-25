using System.Linq;

namespace Unidux.Example.Todo
{
    public static class TodoDuck
    {
        public enum ActionType
        {
            ADD_TODO,
            TOGGLE_TODO,
        }

        public class Action
        {
            public ActionType ActionType;
            public Todo Todo;
        }

        public static State Reducer(State state, Action action)
        {
            switch (action.ActionType)
            {
                case ActionType.ADD_TODO:
                    state.Todo = AddTodo(state.Todo, action.Todo.Text);
                    return state;
                case ActionType.TOGGLE_TODO:
                    state.Todo = ToggleTodo(state.Todo, action.Todo);
                    return state;
            }

            return state;
        }

        public static TodoState AddTodo(TodoState state, string text)
        {
            // TODO: be immutable
            state.List.Add(new Todo()
            {
                Id = state.Index,
                Text = text,
                Completed = false,
            });
            state.Index = state.Index + 1;
            state.SetStateChanged();
            return state;
        }

        public static TodoState ToggleTodo(TodoState state, Todo newTodo)
        {
            var list = state.List.Select(_todo => (_todo.Id == newTodo.Id)
                    ? new Todo()
                    {
                        Id = _todo.Id,
                        Text = _todo.Text,
                        Completed = newTodo.Completed
                    }
                    : _todo
                )
                .ToList();

            state.List = list;
            state.SetStateChanged();
            return state;
        }
    }
}