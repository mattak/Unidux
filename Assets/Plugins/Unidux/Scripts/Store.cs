using System;
using System.Linq;
using UniRx;
using UnityEngine;

namespace Unidux
{
    public class Store<TState> : IStore<TState>, IStoreObject where TState : StateBase
    {
        private TState _state;
        private bool _changed;
        private Subject<TState> _subject;
        private readonly IReducer[] _matchers;
        private Func<object, object> _dispatcher;

        public Subject<TState> Subject
        {
            get { return this._subject = this._subject ?? new Subject<TState>(); }
        }

        public TState State
        {
            get { return this._state; }
            set
            {
                this._changed = this.UpdateStateChanged(this._state, value);
                this._state = value;
            }
        }

        public object ObjectState
        {
            get { return this.State; }
            set { this.State = (TState) value; }
        }

        public UniRx.IObservable<object> ObjectSubject
        {
            get { return this.Subject.Select(it => (object) it); }
        }

        public Type StateType
        {
            get { return typeof(TState); }
        }

        public Store(TState state, params IReducer[] matchers)
        {
            this._state = state;
            this._changed = false;
            this._matchers = matchers ?? new IReducer[0];
        }

        public void ApplyMiddlewares(params Middleware[] middlewares)
        {
            this._dispatcher = (object _action) => { return this._Dispatch(_action); };

            foreach (var middleware in middlewares.Reverse())
            {
                this._dispatcher = middleware(this)(this._dispatcher);
            }
        }

        public object Dispatch(object action)
        {
            if (this._dispatcher == null)
            {
                return this._Dispatch(action);
            }
            else
            {
                return this._dispatcher(action);
            }
        }

        private object _Dispatch(object action)
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

            return null;
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
            bool someStateChanged = true;

            var properties = newState.GetType().GetProperties();
            foreach (var property in properties)
            {
                var newValue = property.GetValue(newState, null);
                var oldValue = property.GetValue(oldState, null);

                if (newValue != null && newValue is IStateChanged)
                {
                    someStateChanged = false;

                    if (!newValue.Equals(oldValue))
                    {
                        ((IStateChanged) newValue).SetStateChanged();
                        stateChanged = true;
                    }
                }
            }
            
            var fields = newState.GetType().GetFields();
            foreach (var field in fields)
            {
                var newValue = field.GetValue(newState);
                var oldValue = field.GetValue(oldState);

                if (newValue != null && newValue is IStateChanged)
                {
                    someStateChanged = false;

                    if (!newValue.Equals(oldValue))
                    {
                        ((IStateChanged) newValue).SetStateChanged();
                        stateChanged = true;
                    }
                }
            }

            // if there is no IStateChanged field, it should be marked as state changed to activate Update function.
            stateChanged |= someStateChanged;

            return stateChanged;
        }

        private void ResetStateChanged(TState state)
        {
            var properties = state.GetType().GetProperties();
            foreach (var property in properties)
            {
                var value = property.GetValue(state, null);
                if (value is IStateChanged)
                {
                    var changedValue = (IStateChanged) value;
                    changedValue.SetStateChanged(false);
                }
            }

            var fields = state.GetType().GetFields();
            foreach (var field in fields)
            {
                var value = field.GetValue(state);
                if (value is IStateChanged)
                {
                    var changedValue = (IStateChanged) value;
                    changedValue.SetStateChanged(false);
                }
            }
        }
    }
}