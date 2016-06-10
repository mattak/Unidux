using System.Text;

namespace Unidux.Example.Todo
{
    public static class TodoReducer
    {
        public static State Reduce(State state, TodoAction action)
        {
            state.TodoList.Add(new TodoEntity(state.TodoList.Count, action.Message));

            var builder = new StringBuilder();
            foreach (var todo in state.TodoList)
            {
                builder.Append(todo.Id)
                    .Append(":")
                    .Append(todo.Message)
                    .Append("\n");
            }

            state.Message = builder.ToString();

            return state;
        }
    }
}
