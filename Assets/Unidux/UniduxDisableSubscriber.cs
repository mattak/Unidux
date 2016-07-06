namespace Unidux
{
    public class UniduxDisableSubscriber : UniduxSubscriberBase
    {
        void OnDisable()
        {
            UnsubscribeRenders();
            DisposeRenders();
        }
    }
}
