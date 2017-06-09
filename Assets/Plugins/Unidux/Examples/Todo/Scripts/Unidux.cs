using UniRx;

namespace Unidux.Example.Todo
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

        private static IReducer[] Reducers
        {
            get { return new IReducer[] {new TodoDuck.Reducer(), new TodoVisibilityDuck.Reducer()}; }
        }

        public static Store<State> Store
        {
            get { return Instance._store = Instance._store ?? new Store<State>(new State(), Reducers); }
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