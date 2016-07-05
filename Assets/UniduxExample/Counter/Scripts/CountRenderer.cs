using UnityEngine;
using UnityEngine.UI;

namespace Unidux.Example.Counter
{
    [RequireComponent(typeof(Text))]
    public class CountRenderer : MonoBehaviour
    {
        void Start()
        {
            var text = this.GetComponent<Text>();
            var store = Unidux.Instance.Store;
            this.gameObject.AddDisableTo(store, state => text.text = state.Count.ToString());
        }
    }
}
