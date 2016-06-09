using System;
using System.Collections.Generic;

namespace Unidux
{
    public class Store<T> : IStore<T> where T : StateBase, new()
    {
        private delegate T ReducerCaller(T state, object action);

        public event Render<T> RenderEvent;
        private readonly Dictionary<Type, ReducerCaller> _reducerDictionary;
        private T _state;
        private bool _changed;

        public T State
        {
            get { return _state ?? (_state = new T()); }
        }

        public Store()
        {
            this._changed = false;
            this._reducerDictionary = new Dictionary<Type, ReducerCaller>();
        }

        public void AddReducer<A>(Reducer<T, A> reducer)
        {
            this._reducerDictionary[typeof(A)] = (state, action) => reducer(state, (A) action);
        }

        public void RemoveReducer<A>(Reducer<T, A> reducer)
        {
            this._reducerDictionary.Remove(typeof(A));
        }

        public void Dispatch<A>(A action)
        {
            foreach (var reducerEntry in this._reducerDictionary)
            {
                var type = reducerEntry.Key;
                var reducer = reducerEntry.Value;

                if (type.Equals(action.GetType()))
                {
                    _state = reducer(State, action);
                    _changed = true;
                }
            }
        }

        public void Update()
        {
            if (!_changed)
            {
                return;
            }

            _changed = false;

            if (RenderEvent != null)
            {
                RenderEvent(State);
            }
        }
    }
}
