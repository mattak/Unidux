using UniRx;

namespace Unidux.Example.Counter
{
    public sealed class Unidux : SingletonMonoBehaviour<Unidux>
    {
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
            get { return Instance._store = Instance._store ?? new Store<State>(new State(), new Count.Reducer()); }
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