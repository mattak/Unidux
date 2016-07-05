namespace Unidux
{
    public class UniduxDisableSubscriber : UniduxSubscriberBase
    {
        void OnEnable()
        {
            CallReducers(true);
            CallRenders(true);
        }

        void OnDisable()
        {
            CallReducers(false);
            CallRenders(false);
        }

        void OnDestroy()
        {
            DisposeReducers();
            DisposeRenders();
        }
    }
}
