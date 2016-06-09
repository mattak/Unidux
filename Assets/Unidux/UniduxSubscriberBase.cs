using System;
using UnityEngine;

namespace Unidux
{
    public class UniduxSubscriberBase : MonoBehaviour, IUniduxSubscriber
    {
        protected Action<bool> _renderSubscriber = (enable) => { };
        protected Action<bool> _reduceSubscriber = (enable) => { };

        public void AddRenderTo<S>(Store<S> store, Render<S> render) where S : StateBase, new()
        {
            _renderSubscriber = (enable) =>
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
            _renderSubscriber(true);
        }

        public void AddReducerTo<S, A>(Store<S> store, Reducer<S, A> render) where S : StateBase, new()
        {
            _reduceSubscriber = (enable) =>
            {
                if (enable)
                {
                    store.AddReducer(render);
                }
                else
                {
                    store.RemoveReducer(render);
                }
            };
            _reduceSubscriber(true);
        }
    }
}
