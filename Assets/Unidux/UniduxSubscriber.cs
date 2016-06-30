namespace Unidux
{
    public class UniduxSubscriber : UniduxSubscriberBase
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
