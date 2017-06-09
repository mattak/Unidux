namespace Unidux
{
    public interface IStateChanged
    {
        bool IsStateChanged { get; }

        void SetStateChanged(bool state);
    }
}