using System;
using System.Collections.Generic;
using System.Linq;
using Unidux.Util;
using UniRx;
using UnityEditor;
using UnityEngine;

namespace Unidux.Experimental.Editor
{
    public partial class UniduxPanelStateTab
    {
        public delegate TResult Func6<in T1, in T2, in T3, in T4, in T5, out TResult>(T1 arg1, T2 arg2, T3 arg3, T4 arg4,
            T5 arg5);

        private Vector2 _scrollPosition = Vector2.zero;
        private Dictionary<string, bool> _foldingMap = new Dictionary<string, bool>();
        private object _newState = null;
        private ISubject<object> _stateSubject;

        public void Render(IStoreAccessor _store)
        {
            if (_store == null)
            {
                EditorGUILayout.HelpBox("Please Set IStoreAccessor", MessageType.Warning);
                return;
            }

            // scrollview of state
            {
                this._scrollPosition = EditorGUILayout.BeginScrollView(this._scrollPosition, GUI.skin.box);
                var state = _store.StoreObject.ObjectState;
                var names = new List<string>();
                var type = state.GetType();

                if (!state.Equals(this._newState))
                {
                    this._newState = CloneUtil.MemoryClone(state);
                }

                var dirty = this.RenderObject(names, state.GetType().Name, this._newState, type, _ => { });
                EditorGUILayout.EndScrollView();

                // XXX: it might be slow and should be updated less frequency.
                if (dirty)
                {
                    _store.StoreObject.ObjectState = this._newState;
                    this._newState = null;
                }
            }
        }

        Func6<List<string>, string, object, Type, Action<object>, bool> SelectObjectRenderer(Type type, object element)
        {
            // struct
            if (type.IsValueType)
            {
                return this.SelectValueRender(type, element);
            }
            else
            {
                return this.SelectClassRender(type, element);
            }
        }

        bool RenderObject(
            List<string> rootNames,
            string name,
            object element,
            Type type,
            Action<object> setter
        )
        {
            return this.SelectObjectRenderer(type, element)(rootNames, name, element, type, setter);
        }

        string GetFoldingName(ICollection<string> collection, string name)
        {
            return name;
        }

        string GetFoldingKey(IEnumerable<string> rootNames)
        {
            return string.Join(".", rootNames.ToArray());
        }
    }
}