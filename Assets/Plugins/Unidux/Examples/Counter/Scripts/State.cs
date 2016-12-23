namespace Unidux.Example.Counter
{
    public class State : StateBase<State>
    {
        public int Count { get; set; }

        public override State Clone()
        {
            var state = (State)MemberwiseClone();
            return state;
        }
    }
}
