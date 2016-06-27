namespace Unidux
{
    public interface IStateChanged
    {
        bool IsStateChanged();

        void SetStateChanged(bool state);
    }
}
