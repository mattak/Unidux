using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace Unidux
{
    public class Store<TState> : IStore<TState> where TState : StateBase<TState>
    {
        private delegate TState ReducerCaller(TState state, object action);

        private readonly Dictionary<Type, ReducerCaller> _reducerDictionary;
        private readonly Dictionary<int, Renderer<TState>> _rendererDictionary;
        private TState _state;
        private bool _changed;

        public TState State
        {
            get { return _state; }
        }

        public Store(TState state)
        {
            this._state = state;
            this._changed = false;
            this._reducerDictionary = new Dictionary<Type, ReducerCaller>();
            this._rendererDictionary = new Dictionary<int, Renderer<TState>>();
        }

        public void AddReducer<TAction>(Reducer<TState, TAction> reducer)
        {
            this._reducerDictionary[typeof(TAction)] = (state, action) => reducer(state, (TAction)action);
        }

        public void RemoveReducer<TAction>(Reducer<TState, TAction> reducer)
        {
            this._reducerDictionary.Remove(typeof(TAction));
        }

        public void AddRenderer(Renderer<TState> renderer)
        {
            int key = renderer.GetHashCode();
            this._rendererDictionary[key] = renderer;
        }

        public void RemoveRenderer(Renderer<TState> renderer)
        {
            int key = renderer.GetHashCode();

            if (this._rendererDictionary.ContainsKey(key))
            {
                this._rendererDictionary.Remove(key);
            }
        }

        public void Dispatch<TAction>(TAction action)
        {
            foreach (var reducerEntry in this._reducerDictionary)
            {
                var type = reducerEntry.Key;
                var reducer = reducerEntry.Value;

                if (type.Equals(action.GetType()))
                {
                    this._state = reducer(this.State, action);
                    this._changed = true;
                }
            }

            if (!this._changed)
            {
                Debug.LogWarning("'Store.Dispatch(" + action + ")' was failed. Maybe you forget to assign reducer.");
            }
        }

        public void ForceUpdate()
        {
            _changed = false;
            TState fixedState;

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
        private void SetNullToOneTimeField(TState state)
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

        private void ResetStateChanged(TState state)
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
