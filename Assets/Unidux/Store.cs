using System;
using System.Linq;
using System.Collections.Generic;

namespace Unidux
{
    public class Store<T> : IStore<T> where T : StateBase<T>
    {
        private delegate T ReducerCaller(T state, object action);

        private readonly Dictionary<Type, ReducerCaller> _reducerDictionary;
        private readonly Dictionary<int, Renderer<T>> _rendererDictionary;
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
            this._rendererDictionary = new Dictionary<int, Renderer<T>>();
        }

        public void AddReducer<A>(Reducer<T, A> reducer)
        {
            this._reducerDictionary[typeof(A)] = (state, action) => reducer(state, (A)action);
        }

        public void RemoveReducer<A>(Reducer<T, A> reducer)
        {
            this._reducerDictionary.Remove(typeof(A));
        }

        public void AddRenderer(Renderer<T> renderer)
        {
            int key = renderer.GetHashCode();
            this._rendererDictionary[key] = renderer;
        }

        public void RemoveRenderer(Renderer<T> renderer)
        {
            int key = renderer.GetHashCode();

            if (this._rendererDictionary.ContainsKey(key))
            {
                this._rendererDictionary.Remove(key);
            }
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

            // NOTE: ToList is important to prevent 'InvalidOperationException: out of sync'
            foreach (var key in this._rendererDictionary.Keys.ToList())
            {
                if (this._rendererDictionary.ContainsKey(key))
                {
                    this._rendererDictionary[key](fixedState);
                }
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
                        UnityEngine.Debug.LogWarning("FlashAfterRenderAttribute does not support primitive type.");
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
