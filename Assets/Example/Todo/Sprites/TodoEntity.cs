using System;

namespace Unidux.Example.Todo
{
    [Serializable]
    public class TodoEntity
    {
        public int Id;
        public string Message;

        public TodoEntity(int id, string message)
        {
            this.Id = id;
            this.Message = message;
        }
    }
}
