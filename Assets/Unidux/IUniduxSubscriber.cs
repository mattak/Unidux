namespace Unidux
{
    public interface IUniduxSubscriber
    {
        void AddRenderTo<S>(Store<S> store, Render<S> render)
            where S : StateBase<S>;

        void AddReducerTo<S, A>(Store<S> store, Reducer<S, A> render)
            where S : StateBase<S>;
    }
}
