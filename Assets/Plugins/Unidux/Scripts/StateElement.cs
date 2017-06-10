using System;

namespace Unidux
{
    [Serializable]
    public class StateElement : IState, IStateChanged
    {
        private bool _stateChanged = false;

        public bool IsStateChanged
        {
            get { return this._stateChanged; }
        }

        public void SetStateChanged(bool changed = true)
        {
            this._stateChanged = changed;
        }
    }
}