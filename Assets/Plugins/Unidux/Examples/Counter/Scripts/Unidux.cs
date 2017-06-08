using UniRx;

namespace Unidux.Example.Counter
{
    public sealed partial class Unidux : SingletonMonoBehaviour<Unidux>
    {
        partial void AddReducers(Store<State> store);

        private Store<State> _store;
        private State _state;

        public static State State
        {
            get { return Instance._state = Instance._state ?? new State(); }
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
                    Instance._store = new Store<State>(State);
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