using UnityEngine;
using System.Collections;
using Unidux.Binder;
using UnityEngine.UI;
using Unidux;

namespace Unidux.Example.Todo
{
    public class ImageRender : MonoBehaviour
    {
        void Start()
        {
            this.GetComponent<Image>().BindPathTo(Unidux.Instance.Store, state => "check");
        }
    }
}
