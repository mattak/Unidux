using System;

namespace Unidux
{
    [Serializable]
    public class StateElement : IState, IStateChanged
    {
        public bool IsStateChanged { get; private set; }

        public void SetStateChanged(bool changed = true)
        {
            this.IsStateChanged = changed;
        }
    }
}