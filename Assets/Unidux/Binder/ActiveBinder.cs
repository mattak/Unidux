using UnityEngine;

namespace Unidux.Binder
{
    public static class ActiveBinder
    {
        public delegate bool BoolBindDelegate<T>(T state) where T : StateBase<T>;

        public static void BindActiveTo<T>(this GameObject gameObject, Store<T> store, BoolBindDelegate<T> caller)
            where T : StateBase<T>
        {
            gameObject.AddDestroyTo(store, state => { gameObject.SetActive(caller(state)); });
        }
    }
}
