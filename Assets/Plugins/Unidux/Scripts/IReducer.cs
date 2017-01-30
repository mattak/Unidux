namespace Unidux
{
    public delegate TState Reducer<TState, in TAction>(TState state, TAction action) where TState : StateBase<TState>;
}