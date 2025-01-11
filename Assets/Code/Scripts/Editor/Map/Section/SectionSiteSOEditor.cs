using UnityEditor;
using UnityEngine;

namespace Com2usGameDev
{
    [CustomEditor(typeof(SectionSiteSO))]
    public class SectionSiteSOEditor : Editor
    {
        private SectionSiteSO site;
        private DefaultAsset tilemapFolder;
        private DefaultAsset dataFolder;

        private void OnEnable()
        {
            site = (SectionSiteSO)target;

            if (!string.IsNullOrEmpty(site.tilemapPath))
            {
                tilemapFolder = AssetDatabase.LoadAssetAtPath<DefaultAsset>(site.tilemapPath);
            }
            if (!string.IsNullOrEmpty(site.dataPath))
            {
                dataFolder = AssetDatabase.LoadAssetAtPath<DefaultAsset>(site.dataPath);
            }
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            EditorGUI.BeginChangeCheck();

            EditorGUILayout.Space(10);
            EditorGUILayout.LabelField("Storage", EditorStyles.boldLabel);
            tilemapFolder = EditorGUILayout.ObjectField("Tilemap", tilemapFolder, typeof(DefaultAsset), false) as DefaultAsset;
            dataFolder = EditorGUILayout.ObjectField("Data", dataFolder, typeof(DefaultAsset), false) as DefaultAsset;

            if (EditorGUI.EndChangeCheck())
            {
                if (tilemapFolder != null)
                    site.tilemapPath = AssetDatabase.GetAssetPath(tilemapFolder);
                if (dataFolder != null)
                    site.dataPath = AssetDatabase.GetAssetPath(dataFolder);

                EditorUtility.SetDirty(target);
            }
        }
    }
}