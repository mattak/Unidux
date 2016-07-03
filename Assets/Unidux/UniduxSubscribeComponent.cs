using UnityEngine;

namespace Unidux
{
    public static class MonoBehaviourExt
    {
        public static void AddTo<T>(
            this GameObject gameObject,
            Store<T> store,
            Render<T> render)
            where T : StateBase<T>
        {
            GetOrAddUniduxSubscriber<UniduxSubscriber>(gameObject).AddRenderTo(store, render);
        }

        public static void AddTo<T, A>(
            this GameObject gameObject,
            Store<T> store,
            Reducer<T, A> reducer)
            where T : StateBase<T>
        {
            GetOrAddUniduxSubscriber<UniduxSubscriber>(gameObject).AddReducerTo(store, reducer);
        }

        public static void AddSustainTo<T>(
            this GameObject gameObject,
            Store<T> store,
            Render<T> render)
            where T : StateBase<T>
        {
            GetOrAddUniduxSubscriber<UniduxSustainSubscriber>(gameObject).AddRenderTo(store, render);
        }

        public static void AddSustainTo<T, A>(
            this GameObject gameObject,
            Store<T> store,
            Reducer<T, A> reducer)
            where T : StateBase<T>
        {
            GetOrAddUniduxSubscriber<UniduxSustainSubscriber>(gameObject).AddReducerTo(store, reducer);
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
