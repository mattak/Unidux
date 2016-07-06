using UnityEngine;

namespace Unidux
{
    public static class MonoBehaviourExtension
    {
        public static void AddDisableTo<T>(
            this MonoBehaviour behaviour,
            Store<T> store,
            Renderer<T> render)
            where T : StateBase<T>
        {
            GetOrAddUniduxSubscriber<UniduxDisableSubscriber>(behaviour.gameObject).AddRenderTo(store, render);
        }

        public static void AddDisableTo<T>(
            this GameObject gameObject,
            Store<T> store,
            Renderer<T> render)
            where T : StateBase<T>
        {
            GetOrAddUniduxSubscriber<UniduxDisableSubscriber>(gameObject).AddRenderTo(store, render);
        }

        public static void AddDestroyTo<T>(
            this MonoBehaviour behaviour,
            Store<T> store,
            Renderer<T> render)
            where T : StateBase<T>
        {
            GetOrAddUniduxSubscriber<UniduxDestroySubscriber>(behaviour.gameObject).AddRenderTo(store, render);
        }

        public static void AddDestroyTo<T>(
            this GameObject gameObject,
            Store<T> store,
            Renderer<T> render)
            where T : StateBase<T>
        {
            GetOrAddUniduxSubscriber<UniduxDestroySubscriber>(gameObject).AddRenderTo(store, render);
        }

        private static T GetOrAddUniduxSubscriber<T>(GameObject gameObject) where T : UniduxSubscriberBase
        {
            var component = gameObject.GetComponent<T>();
            if (component == null)
            {
                gameObject.AddComponent<T>();
                component = gameObject.GetComponent<T>();
            }
            return component;
        }
    }
}
