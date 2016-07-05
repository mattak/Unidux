namespace Unidux
{
    public class UniduxDestroySubscriber : UniduxSubscriberBase
    {
        void Start()
        {
            CallReducers(true);
            CallRenders(true);
        }

        void OnDestroy()
        {
            CallReducers(false);
            CallRenders(false);
            DisposeReducers();
            DisposeRenders();
        }
    }
}
