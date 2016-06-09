using UnityEngine;

namespace Unidux.Binder
{
    public static class ActiveBinder
    {
        public delegate bool BoolBindDelegate<T>(T state) where T : StateBase, new();

        public static void BindActiveTo<T>(this GameObject gameObject, Store<T> store, BoolBindDelegate<T> caller)
            where T : StateBase, new()
        {
            gameObject.AddSustainTo(store, state => { gameObject.SetActive(caller(state)); });
        }
    }
}
