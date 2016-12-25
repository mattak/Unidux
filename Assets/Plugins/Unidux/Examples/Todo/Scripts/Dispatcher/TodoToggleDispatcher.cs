using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Unidux.Example.Todo
{
    public class TodoToggleDispatcher : MonoBehaviour
    {
        public Toggle Toggle;
        public Todo Todo { private get; set; }

        void OnEnable()
        {
            this.Toggle.OnValueChangedAsObservable()
                .TakeUntilDisable(this)
                .Where(_ => Todo != null)
                .DistinctUntilChanged()
                .Where(value => value != Todo.Completed)
                .Subscribe(value => this.Dispatch(value))
                .AddTo(this);
        }

        void Dispatch(bool value)
        {
            Unidux.Store.Dispatch(new TodoDuck.Action()
            {
                ActionType = TodoDuck.ActionType.TOGGLE_TODO,
                Todo =
                    new Todo()
                    {
                        Id = Todo.Id,
                        Text = Todo.Text,
                        Completed = value
                    },
            });
        }
    }
}