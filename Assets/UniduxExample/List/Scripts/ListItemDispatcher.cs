using UnityEngine;
using UnityEngine.UI;

namespace Unidux.Example.List
{
    [RequireComponent(typeof(Button))]
    public class ListItemDispatcher : MonoBehaviour
    {
        public string ItemName = "item";

        void Start()
        {
            var button = this.GetComponent<Button>();
            button.onClick.AddListener(OnClick);
        }

        void OnClick()
        {
            Unidux.Instance.Store.Dispatch(new ListAddAction(ItemName));
        }
    }
}
