namespace Unidux
{
    public interface IStore<S> where S : StateBase<S>
    {
        // TODO: return Subscription
        event Render<S> RenderEvent;
        void AddReducer<A>(Reducer<S, A> reducer);
        void RemoveReducer<A>(Reducer<S, A> reducer);
        void Dispatch<A>(A action);
        void Update();
    }
}
