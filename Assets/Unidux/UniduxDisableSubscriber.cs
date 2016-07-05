namespace Unidux
{
    public class UniduxDisableSubscriber : UniduxSubscriberBase
    {
        void OnDisable()
        {
            UnsubscribeReducers();
            UnsubscribeRenders();
            DisposeReducers();
            DisposeRenders();
        }
    }
}
