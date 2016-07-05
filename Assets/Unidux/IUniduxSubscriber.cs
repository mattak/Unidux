namespace Unidux
{
    public interface IUniduxSubscriber
    {
        void AddRenderTo<S>(Store<S> store, Renderer<S> render)
            where S : StateBase<S>;
    }
}
