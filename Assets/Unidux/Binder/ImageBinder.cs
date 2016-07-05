using UnityEngine;
using UnityEngine.UI;

namespace Unidux.Binder
{
    public static class ImageBinder
    {
        public delegate Sprite SpriteBindDelegate<T>(T state) where T : StateBase<T>;
        public delegate string SpritePathBindDelegate<T>(T state) where T : StateBase<T>;

        public static void BindTo<T>(this Image image, Store<T> store, SpriteBindDelegate<T> caller) where T : StateBase<T>
        {
            image.gameObject.AddDisableTo(store, state => { image.sprite = caller(state); });
        }

        public static void BindPathTo<T>(this Image image, Store<T> store, SpritePathBindDelegate<T> caller) where T : StateBase<T>
        {
            image.gameObject.AddDisableTo(store, state => { image.sprite = Resources.Load<Sprite>(caller(state)); });
        }
    }
}
