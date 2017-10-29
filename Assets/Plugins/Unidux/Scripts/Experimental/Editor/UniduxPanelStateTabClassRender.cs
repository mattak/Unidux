using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Unidux.Util;
using UnityEditor;
using UnityEngine;

namespace Unidux.Experimental.Editor
{
    public partial class UniduxPanelStateTab
    {
        Func6<List<string>, string, object, Type, Action<object>, bool> SelectClassRender(Type type, object element)
        {
            // non struct
            if (type == typeof(string))
            {
                return this.RenderString;
            }
            else if (element is IDictionary)
            {
                return this.RenderDictionary;
            }
            else if (element is IList)
            {
                return this.RenderList;
            }
            else
            {
                return this.RenderClass;
            }
        }

        bool RenderString(List<string> rootNames, string name, object element, Type type, Action<object> setter)
        {
            var oldValue = (element as string);
            var newValue = EditorGUILayout.TextField(name, oldValue);
            var dirty = (oldValue != newValue);

            if (dirty) setter(newValue);
            return dirty;
        }

        bool RenderClass(List<string> rootNames, string name, object element, Type type, Action<object> setter)
        {
            rootNames.Add(name);

            bool dirty = false;
            var foldingKey = this.GetFoldingKey(rootNames);

            this._foldingMap[foldingKey] = EditorGUILayout.Foldout(
                this._foldingMap.GetOrDefault(foldingKey, false),
                this.GetFoldingName(rootNames, name)
            );

            if (this._foldingMap[foldingKey])
            {
                var fields = type.GetFields(BindingFlags.Instance | BindingFlags.Public);
                var properties = type.GetProperties(BindingFlags.Instance | BindingFlags.Public);

                if (fields.Length <= 0 && properties.Length <= 0)
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
                            value,
                            valueType,
                            newValue => field.SetValue(element, newValue));
                    }

                    foreach (var property in properties)
                    {
                        if (property.CanRead && property.CanWrite)
                        {
                            var value = property.GetValue(element, null);
                            var valueType = property.PropertyType;

                            dirty |= RenderObject(
                                rootNames,
                                property.Name,
                                value,
                                valueType,
                                newValue => property.SetValue(element, newValue, null));
                        }
                    }

                    EditorGUILayout.EndVertical();
                }
            }

            rootNames.RemoveLast();
            return dirty;
        }

        bool RenderDictionary(List<string> rootNames, string name, object element, Type type, Action<object> setter)
        {
            rootNames.Add(name);

            IDictionary dictionary = (IDictionary) element;
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
                    var renderer = this.SelectObjectRenderer(valueType, dictionary[keys[0]]);

                    this.RenderPagerContent(
                        foldingKey,
                        keys,
                        (key, index) =>
                        {
                            dirty |= renderer(
                                rootNames,
                                key.ToString(),
                                dictionary[key],
                                valueType,
                                newValue => dictionary[key] = newValue
                            );
                        }
                    );

                    EditorGUILayout.EndVertical();
                }
            }

            rootNames.RemoveLast();
            return dirty;
        }

        bool RenderList(List<string> rootNames, string name, object element, Type type, Action<object> setter)
        {
            rootNames.Add(name);

            IList list = (IList) element;
            bool dirty = false;
            var foldingKey = this.GetFoldingKey(rootNames);

            this._foldingMap[foldingKey] = EditorGUILayout.Foldout(
                this._foldingMap.GetOrDefault(foldingKey, false),
                this.GetFoldingName(rootNames, name)
            );

            if (this._foldingMap[foldingKey])
            {
                if (list.Count <= 0)
                {
                    EditorGUILayout.HelpBox(new StringBuilder(name).Append(" is empty").ToString(), MessageType.Info);
                }
                else
                {
                    EditorGUILayout.BeginVertical(GUI.skin.box);

                    var valueType = list[0].GetType();
                    var render = this.SelectObjectRenderer(valueType, list[0]);

                    this.RenderPagerContent(
                        foldingKey,
                        list,
                        (value, index) =>
                        {
                            var arrayName = new StringBuilder(name).Append("[").Append(index).Append("]").ToString();
                            dirty |= render(
                                rootNames,
                                arrayName,
                                value,
                                valueType,
                                newValue => list[index] = newValue);
                        }
                    );

                    EditorGUILayout.EndVertical();
                }
            }

            rootNames.RemoveLast();
            return dirty;
        }
    }
}