using System;
using System.Collections.Generic;
using UnityEngine;

namespace Unidux
{
    public class UniduxSubscriberBase : MonoBehaviour, IUniduxSubscriber
    {
        private readonly Dictionary<int, Action> _renderSubscriberMap = new Dictionary<int, Action>();
        private readonly Dictionary<int, Action> _reduceSubscriberMap = new Dictionary<int, Action>();

        public void AddRenderTo<S>(Store<S> store, Render<S> render) where S : StateBase<S>
        {
            Action renderSubscriber = null;
            int key = render.GetHashCode();

            if (_renderSubscriberMap.ContainsKey(key))
            {
                renderSubscriber = _renderSubscriberMap[key];
            }
            else
            {
                renderSubscriber = () => { store.RenderEvent -= render; };
            }
            store.RenderEvent += render;

            _renderSubscriberMap[key] = renderSubscriber;
        }

        public void AddReducerTo<S, A>(Store<S> store, Reducer<S, A> reducer) where S : StateBase<S>
        {
            Action reduceSubscriber = null;
            int key = reducer.GetHashCode();

            if (_reduceSubscriberMap.ContainsKey(key))
            {
                reduceSubscriber = _reduceSubscriberMap[key];
            }
            else
            {
                reduceSubscriber = () => { store.RemoveReducer(reducer); };
            }
            store.AddReducer(reducer);

            _reduceSubscriberMap[key] = reduceSubscriber;
        }

        protected void UnsubscribeRenders()
        {
            foreach (var unsubscribe in _renderSubscriberMap.Values)
            {
                unsubscribe();
            }
        }

        protected void UnsubscribeReducers()
        {
            foreach (var unsubscribe in _reduceSubscriberMap.Values)
            {
                unsubscribe();
            }
        }

        protected void DisposeRenders()
        {
            _renderSubscriberMap.Clear();
        }

        protected void DisposeReducers()
        {
            _reduceSubscriberMap.Clear();
        }
    }
}
