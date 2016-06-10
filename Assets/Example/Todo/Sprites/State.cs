using System;
using System.Collections.Generic;

namespace Unidux.Example.Todo
{
    [Serializable]
    public class State : StateBase
    {
        public IList<TodoEntity> TodoList { get; set; }
        public string Message { get; set; }

        [OneTime]
        public string TemporaryMessage { get; set; }

        public State()
        {
            TodoList = new List<TodoEntity>();
            Message = string.Empty;
        }
    }
}
