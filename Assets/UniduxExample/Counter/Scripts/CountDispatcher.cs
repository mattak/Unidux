using UniRx;
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
            this.GetComponent<Button>()
                .OnClickAsObservable()
                .Subscribe(state => Unidux.Store.Dispatch(ActionType))
                .AddTo(this)
                ;
        }
    }
}