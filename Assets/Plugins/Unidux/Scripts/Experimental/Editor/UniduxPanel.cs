using System.IO;
using UnityEditor;
using UnityEngine;

namespace Unidux.Experimental.Editor
{
    public class UniduxPanel : EditorWindow
    {
        [MenuItem("Window/UniduxPanel")]
        static void Init()
        {
            var window = GetWindow<UniduxPanel>();
            window.Show();
        }

        private GameObject _storeObject = null;
        private IStoreAccessor _store = null;
        private int _toolBarPosition = 0;
        private const string DefaultStateJsonPath = "Assets/state.json";
        private const string StoreObjectKey = "UniduxPanel.StoreObject";
        private const string JsonPathKey = "UniduxPanel.JsonSavePath";

        void OnGUI()
        {
            this.titleContent.text = "UniduxPanel";
            EditorGUILayout.LabelField("UniduxEditorWindow", EditorStyles.boldLabel);

            // XXX: ObjectField cannot restrict type selection of IStoreAccessor
            var selectObject =
                EditorGUILayout.ObjectField(
                    "IStoreAccessor",
                    this._storeObject,
                    typeof(GameObject),
                    true
                ) as GameObject;

            // Initial case, but it excepts the case of setting none object.
            if (selectObject == null && this._storeObject == null)
            {
                selectObject = this.LoadObject(StoreObjectKey);
            }

            this._store = selectObject != null ? selectObject.GetComponent<IStoreAccessor>() : null;
            if (this._store != null && this._storeObject != selectObject)
            {
                this.SaveObject(selectObject, StoreObjectKey);
                this._storeObject = selectObject;
            }
            else if (this._store == null && this._storeObject != null)
            {
                this._storeObject = null;
                this.ResetObject(StoreObjectKey);
            }

            // Show toolbar
            this._toolBarPosition = GUILayout.Toolbar(this._toolBarPosition, new[] {"save/load", "state"});

            switch (this._toolBarPosition)
            {
                case 0:
                    this.RenderSettingContent();
                    break;
                case 1:
                    this.RenderStateContent();
                    break;
            }
        }

        private void RenderStateContent()
        {
            if (this._store != null)
            {
                EditorGUILayout.LabelField("TODO: implement");
            }
            else
            {
                EditorGUILayout.HelpBox("Please Set IStoreAccessor", MessageType.Warning);
            }
        }

        private void RenderSettingContent()
        {
            if (this._store == null)
            {
                EditorGUILayout.HelpBox("Please Set IStoreAccessor", MessageType.Warning);
                return;
            }

            var title = "State Json:";
            var jsonPath = EditorPrefs.GetString(JsonPathKey, DefaultStateJsonPath);
            var existsJson = File.Exists(jsonPath);

            EditorGUILayout.LabelField(title, EditorStyles.boldLabel);
            if (existsJson)
            {
                if (GUILayout.Button(jsonPath, EditorStyles.label))
                {
                    var asset = AssetDatabase.LoadAssetAtPath<TextAsset>(jsonPath);
                    Selection.activeObject = asset;
                }
            }
            else
            {
                EditorGUILayout.HelpBox("Create or Select json to save/load", MessageType.Warning);
            }

            EditorGUILayout.BeginHorizontal();
            if (!existsJson && GUILayout.Button("Create"))
            {
                jsonPath = EditorUtility.SaveFilePanel("Create empty json", "Assets", "state.json", "json");
                jsonPath = ReplaceJsonPath(jsonPath);
                if (!string.IsNullOrEmpty(jsonPath))
                {
                    File.WriteAllText(jsonPath, "{}");
                    AssetDatabase.Refresh();
                    RecordJsonPath(jsonPath);
                }
            }

            if (GUILayout.Button("Select"))
            {
                jsonPath = EditorUtility.OpenFilePanel("Select json to save", "", "json");
                jsonPath = ReplaceJsonPath(jsonPath);
                RecordJsonPath(jsonPath);
            }

            if (existsJson && GUILayout.Button("Save"))
            {
                var json = JsonUtility.ToJson(this._store.StateObject);
                File.WriteAllText(jsonPath, json);
                AssetDatabase.Refresh();
            }

            if (existsJson && GUILayout.Button("Load"))
            {
                var content = File.ReadAllText(jsonPath);
                this._store.StateObject = JsonUtility.FromJson(content, this._store.StateType);
            }
            EditorGUILayout.EndHorizontal();
        }

        private void ResetObject(string key)
        {
            EditorPrefs.DeleteKey(key);
        }

        private int SaveObject(GameObject obj, string key)
        {
            if (obj != null)
            {
                int id = obj.GetInstanceID();
                EditorPrefs.SetInt(key, id);
                return id;
            }
            return -1;
        }

        private GameObject LoadObject(string key)
        {
            int id = EditorPrefs.GetInt(key, -1);
            if (id != -1)
            {
                return EditorUtility.InstanceIDToObject(id) as GameObject;
            }
            return null;
        }

        private string ReplaceJsonPath(string path)
        {
            var projectPath = Application.dataPath.Replace("Assets", "");
            return path.Replace(projectPath, "");
        }

        private void RecordJsonPath(string path)
        {
            EditorPrefs.SetString(JsonPathKey, path);
        }
    }
}