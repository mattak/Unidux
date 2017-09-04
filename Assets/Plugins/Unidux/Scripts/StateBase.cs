using System;
using Unidux.Util;

namespace Unidux
{
    [Serializable]
    public class StateBase : IState, IStateClone, IStateChanged
    {
        public virtual TValue Clone<TValue>() where TValue : IStateClone
        {
            return (TValue) StateUtil.MemoryClone(this);
        }

        public bool IsStateChanged { get; private set; }
        
        public void SetStateChanged(bool changed = true)
        {
            this.IsStateChanged = changed;
        }
    }
}