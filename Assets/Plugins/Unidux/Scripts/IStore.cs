using UniRx;

namespace Unidux
{
    public interface IStore<TState> where TState : StateBase<TState>
    {
        TState State { get; }
        Subject<TState> Subject { get; }
        void AddReducer<TAction>(Reducer<TState, TAction> reducer);
        void RemoveReducer<TAction>(Reducer<TState, TAction> reducer);
        void Dispatch<TAction>(TAction action);
        void Update();
    }
}
