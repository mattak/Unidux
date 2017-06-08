using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace Unidux
{
    public class Store<TState> : IStore<TState> where TState : StateBase<TState>
    {
        private delegate TState ReducerCaller(TState state, object action);

        private readonly Dictionary<Type, ReducerCaller> _reducerDictionary;
        private TState _state;
        private bool _changed;
        private Subject<TState> _subject;

        public Subject<TState> Subject
        {
            get { return this._subject = this._subject ?? new Subject<TState>(); }
        }

        public TState State
        {
            get { return _state; }
        }

        public Store(TState state)
        {
            this._state = state;
            this._changed = false;
            this._reducerDictionary = new Dictionary<Type, ReducerCaller>();
        }

        public void AddReducer<TAction>(Reducer<TState, TAction> reducer)
        {
            this._reducerDictionary[typeof(TAction)] = (state, action) => reducer(state, (TAction) action);
        }

        public void RemoveReducer<TAction>(Reducer<TState, TAction> reducer)
        {
            this._reducerDictionary.Remove(typeof(TAction));
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
            this._changed = false;
            TState fixedState;

            lock (this._state)
            {
                // Prevent writing state object
                fixedState = this._state.Clone();

                // The function may slow
                ResetStateChanged(this._state);
            }

            this.Subject.OnNext(fixedState);
        }

        public void Update()
        {
            if (!this._changed)
            {
                return;
            }

            this.ForceUpdate();
        }

        private void ResetStateChanged(TState state)
        {
            var members = state.GetType().GetProperties();
            foreach (var member in members)
            {
                var property = member.GetValue(state, null);
                if (property is IStateChanged)
                {
                    var changedProperty = (IStateChanged) property;
                    changedProperty.SetStateChanged(false);
                }
            }
        }
    }
}