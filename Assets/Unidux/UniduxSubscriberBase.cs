using System;
using System.Collections.Generic;
using UnityEngine;

namespace Unidux
{
    public class UniduxSubscriberBase : MonoBehaviour, IUniduxSubscriber
    {
        private readonly Dictionary<int, Action> _renderSubscriberMap = new Dictionary<int, Action>();

        public void AddRenderTo<S>(Store<S> store, Renderer<S> renderer) where S : StateBase<S>
        {
            Action renderSubscriber = null;
            int key = renderer.GetHashCode();

            if (_renderSubscriberMap.ContainsKey(key))
            {
                renderSubscriber = _renderSubscriberMap[key];
            }
            else
            {
                renderSubscriber = () => { store.RemoveRenderer(renderer); };
            }
            store.AddRenderer(renderer);

            _renderSubscriberMap[key] = renderSubscriber;
        }

        protected void UnsubscribeRenders()
        {
            foreach (var unsubscribe in _renderSubscriberMap.Values)
            {
                unsubscribe();
            }
        }

        protected void DisposeRenders()
        {
            _renderSubscriberMap.Clear();
        }
    }
}
