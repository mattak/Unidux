namespace Unidux
{
    public interface IStore<TState> where TState : StateBase<TState>
    {
        void AddRenderer(Renderer<TState> renderer);
        void RemoveRenderer(Renderer<TState> renderer);
        void AddReducer<TAction>(Reducer<TState, TAction> reducer);
        void RemoveReducer<TAction>(Reducer<TState, TAction> reducer);
        void Dispatch<TAction>(TAction action);
        void Update();
    }
}
