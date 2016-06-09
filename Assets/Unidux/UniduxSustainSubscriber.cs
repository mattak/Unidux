namespace Unidux
{
    public class UniduxSustainSubscriber : UniduxSubscriberBase
    {
        void Start()
        {
            _renderSubscriber(true);
            _reduceSubscriber(true);
        }

        void OnDestroy()
        {
            _renderSubscriber(false);
            _reduceSubscriber(false);
            _renderSubscriber = null;
            _reduceSubscriber = null;
        }
    }
}
