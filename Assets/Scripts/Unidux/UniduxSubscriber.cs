using System;
using UnityEngine;

namespace Unidux
{
    public class UniduxSubscriber : MonoBehaviour
    {
        private Action<bool> renderSubscriber = (enable) => { };
        private Action<bool> reduceSubscriber = (enable) => { };

        void OnEnable()
        {
            renderSubscriber(true);
            reduceSubscriber(true);
        }

        void OnDisable()
        {
            renderSubscriber(false);
            reduceSubscriber(false);
        }

        public void AddRenderTo<S>(Store<S> store, Render<S> render) where S : StateBase, new()
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
            renderSubscriber(true);
        }

        public void AddReducerTo<S>(Store<S> store, Reducer<S> render) where S : StateBase, new()
        {
            reduceSubscriber = (enable) =>
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
            reduceSubscriber(true);
        }
    }
}
