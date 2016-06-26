using System.Collections.Generic;

namespace Unidux.Example.List
{
    public class ListState : StateBase
    {
        public List<string> Texts = new List<string>();
    }

    public class State : StateBase
    {
        public ListState List { get; set; }

        public State()
        {
            this.List = new ListState();
        }
    }
}
