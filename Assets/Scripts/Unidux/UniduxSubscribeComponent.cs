using UnityEngine;

namespace Unidux
{
    public static class MonoBehaviourExt
    {
        public static void AddTo<T>(
            this GameObject gameObject,
            Store<T> store,
            Render<T> render)
            where T : StateBase, new()
        {
            GetOrAddUniduxSubscriber(gameObject).AddRenderTo(store, render);
        }

        public static void AddTo<T>(
            this GameObject gameObject,
            Store<T> store,
            Reducer<T> reducer)
            where T : StateBase, new()
        {
            GetOrAddUniduxSubscriber(gameObject).AddReducerTo(store, reducer);
        }

        private static UniduxSubscriber GetOrAddUniduxSubscriber(GameObject gameObject)
        {
            var component = gameObject.GetComponent<UniduxSubscriber>();
            if (component == null)
            {
                gameObject.AddComponent<UniduxSubscriber>();
                component = gameObject.GetComponent<UniduxSubscriber>();
            }
            return component;
        }
    }
}
