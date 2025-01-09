using UnityEngine;
using UnityEditor;

namespace Com2usGameDev
{
    public static class EditorToolset
    {
        public static T FindSO<T>(string _name) where T : ScriptableObject
        {
            string typeName = typeof(T).ToString();
            string[] guids = AssetDatabase.FindAssets($"{_name} t:{typeName}");
            foreach (string guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                T asset = AssetDatabase.LoadAssetAtPath<T>(path);
                if (asset != null)
                {
                    return asset;
                }
            }
            Debug.Log("Failed.");
            return null;
        }
    }
}
