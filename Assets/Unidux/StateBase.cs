namespace Unidux
{
    public class StateBase : IState, IStateChanged
    {
        public bool StateChanged { get; set; }

        public void SetChanged(bool changed = true)
        {
            this.StateChanged = changed;
        }
    }
}
