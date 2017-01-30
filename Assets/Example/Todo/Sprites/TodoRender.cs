using Unidux.Binder;
using UnityEngine;
using UnityEngine.UI;

namespace Unidux.Example.Todo
{
    public class TodoRender : MonoBehaviour
    {
        void Start()
        {
            var text = this.GetComponent<Text>();
            text.BindTo(Unidux.Instance.Store, state => state.Message);
        }
    }
}
