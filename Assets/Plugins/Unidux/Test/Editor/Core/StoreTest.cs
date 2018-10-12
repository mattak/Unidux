using System;
using System.Collections.Generic;
using NUnit.Framework;
using Unidux.Rx;
using UniRx;

namespace Unidux
{
    public class StoreTest
    {
        [Test]
        public void SubscribeTest()
        {
            var store = new Store<State>(new State());
            var observer = new TestObserver<State>();

            store.Subject.Subscribe(observer);
            Assert.AreEqual(0, observer.OnNextCount);
            Assert.AreEqual(0, observer.OnErrorCount);
            Assert.AreEqual(0, observer.OnCompletedCount);

            store.Dispatch(new SampleAction());
            store.ForceUpdate();

            Assert.AreEqual(1, observer.OnNextCount);
            Assert.AreEqual(0, observer.OnErrorCount);
            Assert.AreEqual(0, observer.OnCompletedCount);
        }

        [Test]
        public void ReducerTest()
        {
            var reducer = new SampleReducer();
            var store = new Store<State>(new State(), reducer);

            Assert.AreEqual(0, reducer.count);
            store.Dispatch(new SampleAction());
            Assert.AreEqual(1, reducer.count);
        }

        [Test]
        public void ResetStateChangedTest()
        {
            var store = new Store<State>(new State());
            var count = 0;

            store.Subject.Subscribe(state =>
            {
                count++;
                Assert.IsTrue(state.ChangedProperty.IsStateChanged);
                Assert.IsTrue(state.ChangedField.IsStateChanged);
            });

            Assert.IsFalse(store.State.ChangedProperty.IsStateChanged);
            Assert.IsFalse(store.State.ChangedField.IsStateChanged);
            store.State.ChangedProperty.SetStateChanged();
            store.State.ChangedField.SetStateChanged();

            store.ForceUpdate();
            Assert.IsFalse(store.State.ChangedProperty.IsStateChanged);
            Assert.IsFalse(store.State.ChangedField.IsStateChanged);
            Assert.AreEqual(1, count);
        }

        [Test]
        public void StateSetTest()
        {
            var store = new Store<State>(new State());
            var count = 0;
            var newState = new State();
            
            store.Subject.Subscribe(state =>
            {
                count++;
                Assert.IsTrue(state.ChangedProperty.IsStateChanged);
                Assert.IsTrue(state.ChangedField.IsStateChanged);
            });
            
            Assert.IsFalse(store.State.ChangedProperty.IsStateChanged);
            Assert.IsFalse(store.State.ChangedField.IsStateChanged);
            
            newState.ChangedField.SetStateChanged();
            newState.ChangedProperty.SetStateChanged();

            store.State = newState;
            
            store.ForceUpdate();
            Assert.IsFalse(store.State.ChangedProperty.IsStateChanged);
            Assert.IsFalse(store.State.ChangedField.IsStateChanged);
            Assert.AreEqual(1, count);
        }

        [Test]
        public void MiddlewareTest()
        {
            var reducer = new SampleReducer();
            var store = new Store<State>(new State(), reducer);
            List<int> positions = new List<int>();

            Middleware middleware1 = _store =>
            {
                return next =>
                {
                    return action =>
                    {
                        positions.Add(1);
                        next(action);
                        positions.Add(2);
                        return "middleware1";
                    };
                };
            };
            Middleware middleware2 = _store =>
            {
                return next =>
                {
                    return action =>
                    {
                        positions.Add(3);
                        next(action);
                        positions.Add(4);
                        return "middleware2";
                    };
                };
            };

            store.ApplyMiddlewares(middleware1);

            Assert.AreEqual("middleware1", store.Dispatch(new SampleAction()));
            Assert.AreEqual(1, reducer.count);
            Assert.AreEqual(2, positions.Count);
            Assert.AreEqual(new[] {1, 2}, positions.ToArray());

            positions.Clear();
            store.ApplyMiddlewares(middleware1, middleware2);

            Assert.AreEqual("middleware1", store.Dispatch(new SampleAction())); 
            Assert.AreEqual(2, reducer.count);
            Assert.AreEqual(4, positions.Count);
            Assert.AreEqual(new[] {1, 3, 4, 2}, positions.ToArray());
        }
        
        [Serializable]
        class State : StateBase
        {
            public ChangedState ChangedProperty { get; set; }
            public ChangedState ChangedField = new ChangedState();

            public State()
            {
                this.ChangedProperty = new ChangedState();
            }
        }

        [Serializable]
        class ChangedState : StateElement
        {
        }

        class SampleAction
        {
            public int Id = 0;
        }

        class SampleReducer : ReducerBase<State, SampleAction>
        {
            public int count = 0;

            public override State Reduce(State state, SampleAction action)
            {
                count++;
                return state;
            }
        }
    }


    public interface IObservablesGet
    {
        IObservable<Unit> Sample(Unit entity);
    }
}