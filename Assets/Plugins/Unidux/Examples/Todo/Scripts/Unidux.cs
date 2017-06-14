using System;
using UniRx;
using UnityEngine;

namespace Unidux.Example.Todo
{
    public sealed class Unidux : SingletonMonoBehaviour<Unidux>, IStoreAccessor
    {
        public TextAsset InitialStateJson;

        private Store<State> _store;

        public static State State
        {
            get { return Store.State; }
        }

        public object StateObject
        {
            get { return (object) State; }
            set { Store.State = (State) value; }
        }

        public Type StateType
        {
            get { return typeof(State); }
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
            get
            {
                var state = (Instance.InitialStateJson != null)
                    ? JsonUtility.FromJson<State>(Instance.InitialStateJson.text)
                    : new State();
                return Instance._store = Instance._store ?? new Store<State>(state, Reducers);
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