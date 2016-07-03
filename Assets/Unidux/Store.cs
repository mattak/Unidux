using System;
using System.Collections.Generic;
using UnityEngine;

namespace Unidux
{
    public class Store<T> : IStore<T> where T : StateBase<T>
    {
        private delegate T ReducerCaller(T state, object action);

        public event Render<T> RenderEvent;

        private readonly Dictionary<Type, ReducerCaller> _reducerDictionary;
        private T _state;
        private bool _changed;

        public T State
        {
            get { return _state; }
        }

        public Store(T state)
        {
            this._state = state;
            this._changed = false;
            this._reducerDictionary = new Dictionary<Type, ReducerCaller>();
        }

        public void AddReducer<A>(Reducer<T, A> reducer)
        {
            this._reducerDictionary[typeof(A)] = (state, action) => reducer(state, (A)action);
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

        public void ForceUpdate()
        {
            _changed = false;
            T fixedState;

            lock (_state)
            {
                // Prevent writing state object
                fixedState = _state.Clone();

                // The function may slow
                SetNullToOneTimeField(_state);
                ResetStateChanged(_state);
            }

            if (RenderEvent != null)
            {
                RenderEvent(fixedState);
            }
        }

        public void Update()
        {
            if (!_changed)
            {
                return;
            }

            ForceUpdate();
        }

        // Experimental feature to flush onetime state value
        private void SetNullToOneTimeField(T state)
        {
            var members = state.GetType().GetProperties();
            foreach (var member in members)
            {
                var attribute = member.GetCustomAttributes(typeof(FlashAfterRenderAttribute), false);

                if (attribute.Length > 0)
                {
                    // Only supports nullable value
                    if (!member.GetType().IsPrimitive)
                    {
                        member.SetValue(state, null, null);
                    }
                    else
                    {
                        Debug.LogWarning("FlashAfterRenderAttribute does not support primitive type.");
                    }
                }
            }
        }

        private void ResetStateChanged(T state)
        {
            var members = state.GetType().GetProperties();
            foreach (var member in members)
            {
                var property = member.GetValue(state, null);
                if (property is IStateChanged)
                {
                    var changedProperty = (IStateChanged)property;
                    changedProperty.SetStateChanged(false);
                }
            }
        }
    }
}
