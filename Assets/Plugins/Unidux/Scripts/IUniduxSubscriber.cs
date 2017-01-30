namespace Unidux
{
    public interface IUniduxSubscriber
    {
        void AddRenderTo<TState>(Store<TState> store, Renderer<TState> render)
            where TState : StateBase<TState>;
    }
}