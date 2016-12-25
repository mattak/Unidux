using UniRx;

namespace Unidux.Example.Todo
{
    public sealed partial class Unidux : SingletonMonoBehaviour<Unidux>
    {
        partial void AddReducers(Store<State> store);

        private ReplaySubject<State> _subject;
        private Store<State> _store;
        private State _state;

        public static State State
        {
            get { return Instance._state = Instance._state ?? new State(); }
        }

        public static ReplaySubject<State> Subject
        {
            get { return Instance._subject = Instance._subject ?? new ReplaySubject<State>(); }
        }

        public static Store<State> Store
        {
            get
            {
                if (Instance._store == null)
                {
                    Instance._store = new Store<State>(State);
                    Instance._store.AddRenderer(state => Subject.OnNext(state));
                    Instance.AddReducers(Instance._store);
                }
                return Instance._store;
            }
        }

        void Update()
        {
            Store.Update();
        }
    }
}
