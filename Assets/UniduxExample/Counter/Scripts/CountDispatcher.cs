using UnityEngine;
using UnityEngine.UI;

namespace Unidux.Example.Counter
{
    [RequireComponent(typeof(Button))]
    public class CountDispatcher : MonoBehaviour
    {
        public Count.ActionType ActionType = Count.ActionType.Increment;

        void Start()
        {
            var button = this.GetComponent<Button>();
            button.onClick.AddListener(() => Unidux.Instance.Store.Dispatch(ActionType));
        }
    }
}
