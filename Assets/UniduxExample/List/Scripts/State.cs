using System.Collections.Generic;

namespace Unidux.Example.List
{
    public class State : StateBase
    {
        public ListState List { get; set; }

        public State()
        {
            this.List = new ListState();
        }
    }
}
