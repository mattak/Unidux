namespace Unidux
{
    public interface IStateClone
    {
        TValue Clone<TValue>() where TValue : IStateClone;
    }
}