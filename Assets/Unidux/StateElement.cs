using UnityEngine;
using System.Collections;

namespace Unidux
{
    public class StateElement : IState, IStateChanged
    {
        private bool _stateChanged = false;

        public bool IsStateChanged()
        {
            return _stateChanged;
        }

        public void SetStateChanged(bool changed = true)
        {
            this._stateChanged = changed;
        }
    }
}