namespace Unidux.Example.List
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
                    _store = new Store<State>(new State());
                    _store.AddReducer<List.Action>(List.Reducer);
                }

                return _store;
            }
        }

        void Update()
        {
            this.Store.ForceUpdate();
        }
    }
}
