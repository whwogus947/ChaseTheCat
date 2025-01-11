using UnityEditor;
using UnityEngine;

namespace Com2usGameDev
{
    public static class GameObjectEditorExtensions
    {
        public static GameObject AsPrefab(this GameObject origin, string path, string name = "", bool focus = false)
        {
            name = string.IsNullOrEmpty(name) ? origin.name : name;
            var prefab = EditorToolset.CreatePrefab(origin, path, name);
            
            if (focus && prefab != null)
            {
                EditorGUIUtility.PingObject(prefab);
                Selection.activeObject = prefab;
            }
            return prefab;
        }
    }
}
