namespace Unidux
{
    public class UniduxDestroySubscriber : UniduxSubscriberBase
    {
        void OnDestroy()
        {
            UnsubscribeRenders();
            DisposeRenders();
        }
    }
}
