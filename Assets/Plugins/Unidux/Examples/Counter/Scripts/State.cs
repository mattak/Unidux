using System;

namespace Unidux.Example.Counter
{
    [Serializable]
    public class State : StateBase
    {
        public int Count { get; set; }
    }
}
