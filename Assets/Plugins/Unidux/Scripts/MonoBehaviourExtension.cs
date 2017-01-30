using UnityEngine;

namespace Unidux
{
    public static class MonoBehaviourExtension
    {
        public static void AddDisableTo<TState>(
            this MonoBehaviour behaviour,
            Store<TState> store,
            Renderer<TState> render)
            where TState : StateBase<TState>
        {
            GetOrAddUniduxSubscriber<UniduxDisableSubscriber>(behaviour.gameObject).AddRenderTo(store, render);
        }

        public static void AddDisableTo<TState>(
            this GameObject gameObject,
            Store<TState> store,
            Renderer<TState> render)
            where TState : StateBase<TState>
        {
            GetOrAddUniduxSubscriber<UniduxDisableSubscriber>(gameObject).AddRenderTo(store, render);
        }

        public static void AddDestroyTo<TState>(
            this MonoBehaviour behaviour,
            Store<TState> store,
            Renderer<TState> render)
            where TState : StateBase<TState>
        {
            GetOrAddUniduxSubscriber<UniduxDestroySubscriber>(behaviour.gameObject).AddRenderTo(store, render);
        }

        public static void AddDestroyTo<TState>(
            this GameObject gameObject,
            Store<TState> store,
            Renderer<TState> render)
            where TState : StateBase<TState>
        {
            GetOrAddUniduxSubscriber<UniduxDestroySubscriber>(gameObject).AddRenderTo(store, render);
        }

        private static TState GetOrAddUniduxSubscriber<TState>(GameObject gameObject)
            where TState : UniduxSubscriberBase
        {
            var component = gameObject.GetComponent<TState>();
            if (component == null)
            {
                gameObject.AddComponent<TState>();
                component = gameObject.GetComponent<TState>();
            }
            return component;
        }
    }
}