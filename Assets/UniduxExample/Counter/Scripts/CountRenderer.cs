using UnityEngine;
using UnityEngine.UI;

namespace Unidux.Example.Counter
{
    [RequireComponent(typeof(Text))]
    public class CountRenderer : MonoBehaviour
    {
        void OnEnable()
        {
            var text = this.GetComponent<Text>();
            var store = Unidux.Instance.Store;
            this.AddDisableTo(store, state => text.text = state.Count.ToString());
        }
    }
}
