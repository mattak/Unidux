using UniRx;

namespace Unidux
{
    public interface IStore<TState> where TState : StateBase
    {
        TState State { get; }
        Subject<TState> Subject { get; }
        
        void Dispatch<TAction>(TAction action);
        void Update();
    }
}
