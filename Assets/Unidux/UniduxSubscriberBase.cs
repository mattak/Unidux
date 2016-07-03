using System;
using System.Collections.Generic;
using UnityEngine;

namespace Unidux
{
    public class UniduxSubscriberBase : MonoBehaviour, IUniduxSubscriber
    {
        private readonly Dictionary<int, Action<bool>> _renderSubscriberMap = new Dictionary<int, Action<bool>>();
        private readonly Dictionary<int, Action<bool>> _reduceSubscriberMap = new Dictionary<int, Action<bool>>();

        public void AddRenderTo<S>(Store<S> store, Render<S> render) where S : StateBase<S>
        {
            Action<bool> renderSubscriber = null;
            int key = render.GetHashCode();

            if (_renderSubscriberMap.ContainsKey(key))
            {
                renderSubscriber = _renderSubscriberMap[key];
            }
            else
            {
                renderSubscriber = (enable) =>
                {
                    if (enable)
                    {
                        store.RenderEvent += render;
                    }
                    else
                    {
                        store.RenderEvent -= render;
                    }
                };
            }
            renderSubscriber(true);

            _renderSubscriberMap[key] = renderSubscriber;
        }

        public void AddReducerTo<S, A>(Store<S> store, Reducer<S, A> reducer) where S : StateBase<S>
        {
            Action<bool> reduceSubscriber = null;
            int key = reducer.GetHashCode();

            if (_reduceSubscriberMap.ContainsKey(key))
            {
                reduceSubscriber = _reduceSubscriberMap[key];
            }
            else
            {
                reduceSubscriber = (enable) =>
                {
                    if (enable)
                    {
                        store.AddReducer(reducer);
                    }
                    else
                    {
                        store.RemoveReducer(reducer);
                    }
                };
            }
            reduceSubscriber(true);

            _reduceSubscriberMap[key] = reduceSubscriber;
        }

        protected void CallRenders(bool subscribe)
        {
            foreach (var caller in _renderSubscriberMap.Values)
            {
                caller(subscribe);
            }
        }

        protected void CallReducers(bool subscribe)
        {
            foreach (var caller in _reduceSubscriberMap.Values)
            {
                caller(subscribe);
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
