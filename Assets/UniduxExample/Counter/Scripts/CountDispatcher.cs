using UnityEngine;
using UnityEngine.UI;

namespace Unidux.Example.Counter
{
    [RequireComponent(typeof(Button))]
    public class CountDispatcher : MonoBehaviour
    {
        public CountAction ActionType = CountAction.Increment;

        void Start()
        {
            var button = this.GetComponent<Button>();
            button.onClick.AddListener(() => Unidux.Instance.Store.Dispatch(ActionType));
        }
    }
}
