namespace Unidux
{
    public class UniduxSustainSubscriber : UniduxSubscriberBase
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
