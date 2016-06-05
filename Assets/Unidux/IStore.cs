namespace Unidux
{
    public interface IStore<S> where S : StateBase
    {
        // TODO: return Subscription
        event Render<S> RenderEvent;
        void AddReducer(Reducer<S> reducer);
        void RemoveReducer(Reducer<S> reducer);
        void Dispatch<A>(A action);
        void Update();
    }
}
