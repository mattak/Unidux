using System.Collections.Generic;

namespace Unidux
{
    public class Store<T> : IStore<T> where T : StateBase, new()
    {
        public event Render<T> RenderEvent;
        private readonly IList<Reducer<T>> _reducerList;
        private T _state;
        private bool changed;

        public T State
        {
            get
            {
                if (_state == null)
                {
                    _state = new T();
                }

                return _state;
            }
        }

        public Store()
        {
            this.changed = false;
            this._reducerList = new List<Reducer<T>>();
        }

        public void AddReducer(Reducer<T> reducer)
        {
            this._reducerList.Add(reducer);
        }

        public void RemoveReducer(Reducer<T> reducer)
        {
            this._reducerList.Remove(reducer);
        }

        public void Dispatch<A>(A action)
        {
            foreach (var _reducer in _reducerList)
            {
                _state = _reducer(State, action);
            }

            changed = true;
        }

        public void Update()
        {
            if (!changed)
            {
                return;
            }

            changed = false;

            if (RenderEvent != null)
            {
                RenderEvent(State);
            }
        }
    }
}
