using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Unidux.Experimental.Editor
{
    [InitializeOnLoad]
    public class StateJsonEditor
    {
        /// <summary>
        /// Unidux specific extension is defined for avoiding conflicts of UnityEngine or the other assets json handling.
        /// `.unidux_json` extension is defined for using EditorUtility.SaveFilePanel.
        /// Because the function cannot handle double dot filename such as `.unidux.json`.
        /// </summary>
        private static readonly string[] FILE_EXTENSIONS = new[] { "unidux_json", "unidux.json" };

        private static StateJsonFileWrapper wrapper = null;
        private static bool selectionChanged = false;

        static StateJsonEditor()
        {
            Selection.selectionChanged += SelectionChanged;
            EditorApplication.update += Update;
        }

        private static void SelectionChanged()
        {
            selectionChanged = true;
            // can't do the wrapper stuff here. it does not work 
            // when you Selection.activeObject = wrapper
            // so do it in Update
        }

        private static void Update()
        {
            if (selectionChanged == false) return;

            selectionChanged = false;

            if (Selection.activeObject != wrapper)
            {
                string fn = AssetDatabase.GetAssetPath(Selection.activeInstanceID);
                if (FILE_EXTENSIONS.Any(extension => fn.ToLower().EndsWith(extension)))
                {
                    if (wrapper == null)
                    {
                        wrapper = ScriptableObject.CreateInstance<StateJsonFileWrapper>();
                        wrapper.hideFlags = HideFlags.DontSave;
                    }

                    wrapper.FileName = fn;
                    Selection.activeObject = wrapper;

                    UnityEditor.Editor[] editor = Resources.FindObjectsOfTypeAll<StateJsonFileWrapperInspector>();

                    if (editor.Length > 0)
                    {
                        editor[0].Repaint();
                    }
                }
            }
        }
    }
}