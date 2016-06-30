namespace Unidux.Example.Counter
{
    public class Unidux : SingletonMonoBehaviour<Unidux>
    {
        private Store<State> _store;
        public Store<State> Store
        {
            get
            {
                if (null == _store)
                {
                    _store = new Store<State>();
                    _store.AddReducer<CountAction>(CountReducer.Reduce);
                }
                return _store;
            }
        }

        void Update()
        {
            this.Store.Update();
        }
    }
}
