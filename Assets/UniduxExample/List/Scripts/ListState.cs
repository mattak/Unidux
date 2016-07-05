using System.Collections.Generic;

namespace Unidux.Example.List
{
    public class ListState : StateElement<ListState>
    {
        public List<string> Texts = new List<string>();
    }
}
