using UniRx;

namespace Unidux.Example.List
{
    public sealed partial class Unidux : SingletonMonoBehaviour<Unidux>
    {
        partial void AddReducers(Store<State> store);

        private Store<State> _store;

        public static State State
        {
            get { return Store.State; }
        }
        
        public static Subject<State> Subject
        {
            get { return Store.Subject; }
        }

        public static Store<State> Store
        {
            get
            {
                if (Instance._store == null)
                {
                    Instance._store = new Store<State>(new State());
                    Instance.AddReducers(Instance._store);
                }
                return Instance._store;
            }
        }

        public static void Dispatch<TAction>(TAction action)
        {
            Store.Dispatch(action);
        }

        void Update()
        {
            Store.Update();
        }
    }
}
