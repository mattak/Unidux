namespace Unidux.Example.Counter
{
    // Action
    public enum CountAction
    {
        Increment,
        Decrement
    }

    public static class CountReducer
    {
        // Reducer
        public static State Reducer(State state, CountAction action)
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

        // ActionCreator
        public static CountAction Increment()
        {
            return CountAction.Increment;
        }
    }
}
