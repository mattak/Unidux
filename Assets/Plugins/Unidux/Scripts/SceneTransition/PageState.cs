using System;
using System.Collections.Generic;
using System.Linq;

namespace Unidux.SceneTransition
{
    [Serializable]
    public class PageState<TPage> : StateElement, ICloneable where TPage : struct
    {
        // XXX: System.Collections.Generic.Stack is not visible on UniduxEditorStateTab, because it's not writable on dynamically. So use IList instead of Stack now,
        public readonly IList<IPageEntity<TPage>> Stack = new List<IPageEntity<TPage>>();

        public IPageEntity<TPage> Current
        {
            get { return this.Stack.Last(); }
        }

        public TPage CurrentPage
        {
            get { return this.Current.Page; }
        }

        public IPageData CurrentData
        {
            get { return this.Current.Data; }
        }

        public TData GetCurrentData<TData>() where TData : IPageData
        {
            return (TData) this.CurrentData;
        }

        public bool IsReady
        {
            get { return this.Stack.Count > 0; }
        }

        public PageState()
        {
        }

        public PageState(PageState<TPage> state)
        {
            foreach(var page in state.Stack)
            {
                this.Stack.Add(page);
            }
        }

        public object Clone()
        {
            return new PageState<TPage>(this);
        }

        public override bool Equals(object obj)
        {
            if (obj is PageState<TPage>)
            {
                var target = (PageState<TPage>) obj;
                return this.Stack.SequenceEqual(target.Stack);
            }

            return false;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}