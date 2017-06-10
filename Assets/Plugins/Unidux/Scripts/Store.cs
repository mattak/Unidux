using UniRx;
using UnityEngine;

namespace Unidux
{
    public class Store<TState> : IStore<TState> where TState : StateBase
    {
        private TState _state;
        private bool _changed;
        private Subject<TState> _subject;
        private IReducer[] _matchers;

        public Subject<TState> Subject
        {
            get { return this._subject = this._subject ?? new Subject<TState>(); }
        }

        public TState State
        {
            get { return _state; }
        }

        public Store(TState state, params IReducer[] matchers)
        {
            this._state = state;
            this._changed = false;
            this._matchers = matchers ?? new IReducer[0];
        }
        
        public void Dispatch<TAction>(TAction action)
        {
            foreach (var matcher in this._matchers)
            {
                if (matcher.IsMatchedAction(action))
                {
                    this._state = (TState)matcher.ReduceAny(this.State, action);
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
                fixedState = this._state.Clone<TState>();

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