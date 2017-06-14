using UniRx;
using UnityEngine;

namespace Unidux
{
    public class Store<TState> : IStore<TState> where TState : StateBase
    {
        private TState _state;
        private bool _changed;
        private Subject<TState> _subject;
        private readonly IReducer[] _matchers;

        public Subject<TState> Subject
        {
            get { return this._subject = this._subject ?? new Subject<TState>(); }
        }

        public TState State
        {
            get { return _state; }
            set
            {
                this._changed = this.UpdateStateChanged(this._state, value);
                this._state = value;
            }
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
                    this._state = (TState) matcher.ReduceAny(this.State, action);
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

        private bool UpdateStateChanged(TState oldState, TState newState)
        {
            bool stateChanged = false;
            bool hasNoStateChangedFields = true;
            var fields = newState.GetType().GetFields();

            foreach (var field in fields)
            {
                var newValue = field.GetValue(newState);
                var oldValue = field.GetValue(oldState);

                if (newValue != null && newValue is IStateChanged)
                {
                    hasNoStateChangedFields = false;

                    if (!newValue.Equals(oldValue))
                    {
                        ((IStateChanged) newValue).SetStateChanged();
                        stateChanged = true;
                    }
                }
            }

            // if there is no IStateChanged field, it should be marked as state changed to activate Update function.
            stateChanged |= hasNoStateChangedFields;

            return stateChanged;
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