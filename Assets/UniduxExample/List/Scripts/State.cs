namespace Unidux.Example.List
{
    public class State : StateBase<State>
    {
        public ListState List { get; private set; }

        public State()
        {
            this.List = new ListState();
        }

        public override State Clone()
        {
            var state = (State)MemberwiseClone();
            state.List = state.List.Clone();
            return state;
        }
    }
}
