using Unidux.Binder;
using UnityEngine;

namespace Unidux.Example.Todo
{
    public class AlertRender : MonoBehaviour
    {
        void Start()
        {
            this.gameObject.BindActiveTo(Unidux.Instance.Store, state => state.TodoList.Count > 3);
        }
    }
}
