using UnityEngine;
using UnityEditor;
using System.IO;

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

        public static void CreatePrefab(GameObject target, string assetCreationPath, string prefabName)
        {
            if (!Directory.Exists(assetCreationPath))
            {
                Directory.CreateDirectory(assetCreationPath);
            }

            string prefabPath = Path.Combine(assetCreationPath, prefabName + ".prefab");

            bool overwrite;
            if (File.Exists(prefabPath))
            {
                overwrite = EditorUtility.DisplayDialog(
                    "Overwrite Prefab",
                    $"Prefab '{prefabName}' already exists. Do you want to overwrite it?",
                    "Yes",
                    "No"
                );

                if (!overwrite)
                {
                    Debug.Log("Prefab creation cancelled.");
                    return;
                }
            }

            GameObject prefab = PrefabUtility.SaveAsPrefabAsset(target, prefabPath);

            if (prefab != null)
            {
                Debug.Log($"Prefab successfully created: {prefabPath}");
                Selection.activeObject = prefab;
            }
            else
            {
                Debug.LogError("Failed to create prefab.");
            }
        }
    }
}