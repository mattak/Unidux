using UnityEngine.UI;

namespace Unidux.Binder
{
    public static class TextBinder
    {
        public delegate string StringBindDelegate<T>(T state) where T : StateBase, new();

        public static void BindTo<T>(this Text text, Store<T> store, StringBindDelegate<T> caller) where T : StateBase, new()
        {
            text.gameObject.AddTo<T>(store, state => { text.text = caller(state); });
        }
    }
}
