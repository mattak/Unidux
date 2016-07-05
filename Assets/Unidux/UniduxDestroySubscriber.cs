namespace Unidux
{
    public class UniduxDestroySubscriber : UniduxSubscriberBase
    {
        void OnDestroy()
        {
            UnsubscribeReducers();
            UnsubscribeRenders();
            DisposeReducers();
            DisposeRenders();
        }
    }
}
