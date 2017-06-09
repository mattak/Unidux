namespace Unidux
{
    public abstract class ReducerBase<TState, TAction> : IReducer
    {
        public abstract TState Reduce(TState state, TAction action);

        public object ReduceAny(object state, object action)
        {
            return this.Reduce((TState) state, (TAction) action);
        }

        public bool IsMatchedAction(object action)
        {
            return action is TAction;
        }
    }
}