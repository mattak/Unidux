using System.Collections.Generic;
using System.Linq;

namespace Unidux.Example.Todo
{
    public class State : StateBase<State>
    {
        public TodoState Todo { get; set; }

        public State()
        {
            this.Todo = new TodoState();
        }

        public override State Clone()
        {
            State state = new State();
            state.Todo = this.Todo.Clone();
            return state;
        }
    }

    public class TodoState : StateElement<TodoState>
    {
        public uint Index = 0;
        public List<Todo> List = new List<Todo>();
        public VisibilityFilter Filter = VisibilityFilter.All;

        public List<Todo> ListByFilter
        {
            get
            {
                return this.List.Where(todo =>
                        this.Filter == VisibilityFilter.All ||
                        (this.Filter == VisibilityFilter.Active && !todo.Completed) ||
                        (this.Filter == VisibilityFilter.Completed && todo.Completed)
                    )
                    .ToList();
            }
        }
    }
}