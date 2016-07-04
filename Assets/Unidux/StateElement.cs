namespace Unidux
{
    public class StateElement<T> : IState, IStateChanged, IStateClone<T>
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

        public T Clone()
        {
            return (T)MemberwiseClone();
        }
    }
}
