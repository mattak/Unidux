using System;

namespace Unidux.Example.List
{
    [Serializable]
    public class State : StateBase
    {
        public ListState List { get; private set; }

        public State()
        {
            this.List = new ListState();
        }
    }
}
