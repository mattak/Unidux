using UniRx;
using UnityEngine;

namespace Unidux.Example.Middlewares
{
    public class Unidux : SingletonMonoBehaviour<Unidux>
    {
        public TextAsset InitialStateJson;

        private Store<State> _store;

        public IStoreObject StoreObject
        {
            get { return Store; }
        }

        public static State State
        {
            get { return Store.State; }
        }

        public static Subject<State> Subject
        {
            get { return Store.Subject; }
        }

        private static State InitialState
        {
            get
            {
                return Instance.InitialStateJson != null
                    ? JsonUtility.FromJson<State>(Instance.InitialStateJson.text)
                    : new State();
            }
        }

        public static Store<State> Store
        {
            get
            {
                return Instance._store =
                    Instance._store ?? new Store<State>(InitialState, new MiddlewareDuck.Reducer());
            }
        }

        public static object Dispatch<TAction>(TAction action)
        {
            return Store.Dispatch(action);
        }

        void Start()
        {
            Store.ApplyMiddlewares(
                Middlewares.Thunk,
                Middlewares.Logger,
                Middlewares.CrashReport
            );
        }

        void Update()
        {
            Store.Update();
        }
    }
}