using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;

namespace Com2usGameDev
{
    public static class EditorToolset
    {
        public static T Find<T>(string _name) where T : Object
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
            Debug.LogWarning($"empty {_name}");
            return null;
        }

        public static T[] FindAll<T>() where T : Object
        {
            string[] guids = AssetDatabase.FindAssets("t:" + typeof(T).Name);
            T[] assets = new T[guids.Length];

            for (int i = 0; i < guids.Length; i++)
            {
                string path = AssetDatabase.GUIDToAssetPath(guids[i]);
                assets[i] = AssetDatabase.LoadAssetAtPath<T>(path);
            }
            return assets;
        }

        public static GameObject[] FindAllPrefab<T>(string folderPath) where T : Component
        {
            string[] guids = AssetDatabase.FindAssets("t:Prefab", new[] { folderPath });
            List<GameObject> assets = new();

            for (int i = 0; i < guids.Length; i++)
            {
                string path = AssetDatabase.GUIDToAssetPath(guids[i]);
                var asset = AssetDatabase.LoadAssetAtPath<GameObject>(path);
                if (asset.GetComponent<T>() != null)
                {
                    assets.Add(asset );
                }
            }
            return assets.ToArray();
        }

        public static GameObject CreatePrefab(GameObject target, string assetCreationPath, string prefabName)
        {
            if (!Directory.Exists(assetCreationPath))
            {
                Directory.CreateDirectory(assetCreationPath);
            }

            string finalPrefabName = prefabName;
            string prefabPath = Path.Combine(assetCreationPath, finalPrefabName + ".prefab");
            int index = 1;

            while (File.Exists(prefabPath))
            {
                finalPrefabName = $"{prefabName} ({index})";
                prefabPath = Path.Combine(assetCreationPath, finalPrefabName + ".prefab");
                index++;
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
            return prefab;
        }
    }
}