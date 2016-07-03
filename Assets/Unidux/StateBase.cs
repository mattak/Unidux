namespace Unidux
{
    public class StateBase<T> : IState, IStateClone<T>
    {
        public T Clone()
        {
            return (T) MemberwiseClone();
        }
    }
}
