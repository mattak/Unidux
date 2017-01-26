using System;

namespace Unidux.Example.Counter
{
    [Serializable]
    public class State : StateBase<State>
    {
        public int Count { get; set; }
    }
}
