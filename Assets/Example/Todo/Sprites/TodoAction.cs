using System;

namespace Unidux.Example.Todo
{
    [Serializable]
    public class TodoAction
    {
        public string Message;

        public TodoAction(string message)
        {
            this.Message = message;
        }
    }
}
