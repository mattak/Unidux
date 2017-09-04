using System;
using Unidux.Util;

namespace Unidux
{
    [Serializable]
    public class StateBase : IState, IStateChanged, ICloneable
    {
        public virtual object Clone()
        {
            // It's slow. in case of requiring performance override this deep clone method by your code.
            return StateUtil.MemoryClone(this);
        }

        public bool IsStateChanged { get; private set; }
        
        public void SetStateChanged(bool changed = true)
        {
            this.IsStateChanged = changed;
        }
    }
}