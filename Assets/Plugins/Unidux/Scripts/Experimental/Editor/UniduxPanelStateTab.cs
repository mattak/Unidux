using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Unidux.Util;
using UniRx;
using UnityEditor;
using UnityEngine;

namespace Unidux.Experimental.Editor
{
    public class UniduxPanelStateTab
    {
        private Vector2 _scrollPosition = Vector2.zero;
        private Dictionary<string, int> _choiceIndexMap = new Dictionary<string, int>();
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
                    this._newState = StateUtil.MemoryClone(state);
                }

                var dirty = this.RenderObject(names, state.GetType().Name, type, this._newState, _ => { });
                EditorGUILayout.EndScrollView();

                // XXX: it might be slow and should be updated less frequency.
                if (dirty)
                {
                    UnityEngine.Debug.Log("dirty");
                    _store.StoreObject.ObjectState = this._newState;
                    this._newState = null;
                }
            }
        }

        bool RenderObject(
            List<string> rootNames,
            string name,
            Type type,
            object element,
            Action<object> setter
        )
        {
            bool dirty = false;

            // PRIMITIVE
            if (element is int)
            {
                var oldValue = (int) element;
                var newValue = EditorGUILayout.IntField(name, oldValue);
                dirty |= (oldValue != newValue);

                if (dirty) setter(newValue);
            }
            else if (element is uint)
            {
                var oldValue = (int) ((uint) element);
                var newValue = EditorGUILayout.IntField(name, oldValue);
                dirty |= (oldValue != newValue);

                if (newValue < 0) Debug.LogWarning("Illeagal uint value setted: " + newValue);
                else if (dirty) setter((uint) newValue);
            }
            else if (element is float)
            {
                var oldValue = (float) element;
                var newValue = EditorGUILayout.FloatField(name, oldValue);
                dirty |= (oldValue != newValue);

                if (dirty) setter(newValue);
            }
            else if (element is double)
            {
                var oldValue = (double) element;
                var newValue = EditorGUILayout.DoubleField(name, oldValue);
                dirty |= (oldValue != newValue);

                if (dirty) setter(newValue);
            }
            else if (element is long)
            {
                var oldValue = (long) element;
                var newValue = EditorGUILayout.LongField(name, oldValue);
                dirty |= (oldValue != newValue);

                if (dirty) setter(newValue);
            }
            else if (element is ulong)
            {
                var oldValue = (long) ((ulong) element);
                var newValue = EditorGUILayout.LongField(name, oldValue);
                dirty |= (oldValue != newValue);

                if (newValue < 0) Debug.LogWarning("Illeagal ulong value setted: " + newValue);
                else if (dirty) setter((ulong) newValue);
            }
            else if (element is bool)
            {
                var oldValue = (bool) element;
                var newValue = EditorGUILayout.Toggle(name, oldValue);
                dirty |= (oldValue != newValue);

                if (dirty) setter(newValue);
            }
            // NON PRIMITIVE
            else if (type == typeof(string))
            {
                var oldValue = (element as string);
                var newValue = EditorGUILayout.TextField(name, oldValue);
                dirty |= (oldValue != newValue);

                if (dirty) setter(newValue);
            }
            else if (type.IsEnum)
            {
                string[] _choices = Enum.GetNames(type);
                var foldingKey = this.GetFoldingKey(rootNames);
                var oldValue = _choiceIndexMap.GetOrDefault(foldingKey, 0);
                var newValue = EditorGUILayout.Popup(name, oldValue, _choices);
                _choiceIndexMap[foldingKey] = newValue;
                dirty |= (oldValue != newValue);

                if (dirty) setter(newValue);
            }
            else if (element is Color)
            {
                var oldValue = (Color) element;
                var newValue = EditorGUILayout.ColorField(name, oldValue);
                dirty |= (oldValue != newValue);

                if (dirty) setter(newValue);
            }
            else if (element is Vector2)
            {
                var oldValue = (Vector2) element;
                var newValue = EditorGUILayout.Vector2Field(name, oldValue);
                dirty |= (oldValue != newValue);

                if (dirty) setter(newValue);
            }
            else if (element is Vector3)
            {
                var oldValue = (Vector3) element;
                var newValue = EditorGUILayout.Vector3Field(name, oldValue);
                dirty |= (oldValue != newValue);

                if (dirty) setter(newValue);
            }
            else if (element is Vector4)
            {
                var oldValue = (Vector4) element;
                var newValue = EditorGUILayout.Vector4Field(name, oldValue);
                dirty |= (oldValue != newValue);

                if (dirty) setter(newValue);
            }
            else if (element is IDictionary)
            {
                rootNames.Add(name);

                dirty |= RenderDictionary(rootNames, name, type, (IDictionary) element);

                rootNames.RemoveLast();
            }
            else if (element is IList)
            {
                rootNames.Add(name);

                dirty |= RenderList(rootNames, name, type, (IList) element);

                rootNames.RemoveLast();
            }
            else if (!type.IsPrimitive)
            {
                rootNames.Add(name);

                dirty |= RenderClass(rootNames, name, type, element);

                rootNames.RemoveLast();
            }
            else
            {
                Debug.LogWarning("UniduxPanel is unsupporting the Primitive Type: " + type);
            }

            return dirty;
        }

        string GetFoldingName(ICollection<string> collection, string name)
        {
            return name;
        }

        string GetFoldingKey(List<string> rootNames)
        {
            return string.Join(".", rootNames.ToArray());
        }

        bool RenderClass(List<string> rootNames, string name, Type type, object element)
        {
            bool dirty = false;
            var foldingKey = this.GetFoldingKey(rootNames);

            this._foldingMap[foldingKey] = EditorGUILayout.Foldout(
                this._foldingMap.GetOrDefault(foldingKey, false),
                this.GetFoldingName(rootNames, name)
            );

            if (this._foldingMap[foldingKey])
            {
                var fields = type.GetFields(BindingFlags.Instance | BindingFlags.Public);

                if (fields.Length <= 0)
                {
                    EditorGUILayout.HelpBox(new StringBuilder(name).Append(" has no properties").ToString(),
                        MessageType.Info);
                }
                else
                {
                    EditorGUILayout.BeginVertical(GUI.skin.box);
                    foreach (var field in fields)
                    {
                        var value = field.GetValue(element);
                        var valueType = field.FieldType;
                        dirty |= RenderObject(
                            rootNames,
                            field.Name,
                            valueType,
                            value,
                            newValue => field.SetValue(element, newValue));
                    }
                    EditorGUILayout.EndVertical();
                }
            }

            return dirty;
        }

        bool RenderList(List<string> rootNames, string name, Type type, IList element)
        {
            bool dirty = false;
            var index = 0;
            var foldingKey = this.GetFoldingKey(rootNames);

            this._foldingMap[foldingKey] = EditorGUILayout.Foldout(
                this._foldingMap.GetOrDefault(foldingKey, false),
                this.GetFoldingName(rootNames, name)
            );

            if (this._foldingMap[foldingKey])
            {
                if (element.Count <= 0)
                {
                    EditorGUILayout.HelpBox(new StringBuilder(name).Append(" is empty").ToString(), MessageType.Info);
                }
                else
                {
                    EditorGUILayout.BeginVertical(GUI.skin.box);

                    var valueType = element[0].GetType();

                    foreach (var e in element)
                    {
                        var arrayName = new StringBuilder(name).Append("[").Append(index).Append("]").ToString();
                        dirty |= RenderObject(
                            rootNames,
                            arrayName,
                            valueType,
                            e,
                            newValue => element[index] = newValue);
                        index++;
                    }
                    EditorGUILayout.EndVertical();
                }
            }

            return dirty;
        }

        bool RenderDictionary(List<string> rootNames, string name, Type type, IDictionary dictionary)
        {
            bool dirty = false;
            var valueTypes = dictionary.Values.GetType().GetGenericArguments();
            var valueType = valueTypes[1];
            var foldingKey = this.GetFoldingKey(rootNames);

            this._foldingMap[foldingKey] = EditorGUILayout.Foldout(
                this._foldingMap.GetOrDefault(foldingKey, false),
                name
            );

            if (this._foldingMap[foldingKey])
            {
                if (dictionary.Count <= 0)
                {
                    EditorGUILayout.HelpBox(new StringBuilder(name).Append(" is empty").ToString(), MessageType.Info);
                }
                else
                {
                    EditorGUILayout.BeginVertical(GUI.skin.box);
                    IList keys = new ArrayList(dictionary.Keys);

                    foreach (var key in keys)
                    {
                        dirty |= RenderObject(
                            rootNames,
                            key.ToString(),
                            valueType,
                            dictionary[key],
                            newValue => dictionary[key] = newValue
                        );
                    }

                    if (dirty)
                    {
                        UnityEngine.Debug.Log("Dictionary is dirty");
                    }

                    EditorGUILayout.EndVertical();
                }
            }

            return dirty;
        }
    }
}