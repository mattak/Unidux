namespace Unidux
{
    public abstract class StateBase<T> : IState, IStateClone<T>
    {
        public abstract T Clone();
    }
}
