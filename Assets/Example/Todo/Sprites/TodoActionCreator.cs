using UnityEngine;
using UnityEngine.UI;

namespace Unidux.Example.Todo
{
    public class TodoActionCreator : MonoBehaviour
    {
        void Start()
        {
            this.gameObject.AddTo<State, TodoAction>(Unidux.Instance.Store, TodoReducer.Reduce);

            var field = GetComponent<InputField>();
            field.onEndEdit.AddListener(text => { Unidux.Instance.Store.Dispatch(new TodoAction(text)); });
            // field.onEndEdit.Dispatch(Unidux.Instance.Store, text => new TodoAction(text));
        }
    }
}
