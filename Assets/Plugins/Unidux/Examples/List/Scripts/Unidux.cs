using System;
using UniRx;
using UnityEngine;

namespace Unidux.Example.List
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
            get { return Store.State; }
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
            get { return Instance._store = Instance._store ?? new Store<State>(InitialState, new List.Reducer()); }
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