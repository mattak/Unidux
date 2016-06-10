namespace Unidux.Example.Todo
{
    public class Unidux : SingletonMonoBehaviour<Unidux>
    {
        private Store<State> _store;

        public Store<State> Store
        {
            get
            {
                if (_store == null)
                {
                    _store = new Store<State>();
                }

                return _store;
            }
        }

        void Update()
        {
            _store.Update();
        }
    }
}
