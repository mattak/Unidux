namespace Unidux.Example.Counter
{
    public static class CountReducer
    {
        public static State Reduce(State state, CountAction action)
        {
            switch (action)
            {
                case CountAction.Increment:
                    state.Count++;
                    break;
                case CountAction.Decrement:
                    state.Count--;
                    break;
            }

            return state;
        }
    }
}
