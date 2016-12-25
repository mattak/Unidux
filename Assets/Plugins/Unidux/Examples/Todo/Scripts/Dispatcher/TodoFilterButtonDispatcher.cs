using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Unidux.Example.Todo
{
    [RequireComponent(typeof(Button))]
    public class TodoFilterButtonDispatcher : MonoBehaviour
    {
        public VisibilityFilter Filter;

        void OnEnable()
        {
            this.GetComponent<Button>()
                .OnClickAsObservable()
                .TakeUntilDisable(this)
                .Subscribe(_ => Unidux.Store.Dispatch(new TodoVisibilityDuck.Action()
                {
                    ActionType = TodoVisibilityDuck.ActionType.SET_VISIBILITY,
                    Filter = Filter
                }))
                .AddTo(this);
        }
    }
}