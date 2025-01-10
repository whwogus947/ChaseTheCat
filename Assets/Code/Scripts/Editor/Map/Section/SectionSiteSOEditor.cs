using UnityEditor;
using UnityEngine;

namespace Com2usGameDev
{
    [CustomEditor(typeof(SectionSiteSO))]
    public class SectionSiteSOEditor : Editor
    {
        private SectionSiteSO site;
        private DefaultAsset targetFolder;

        private void OnEnable()
        {
            site = (SectionSiteSO)target;

            if (!string.IsNullOrEmpty(site.folderPath))
            {
                targetFolder = AssetDatabase.LoadAssetAtPath<DefaultAsset>(site.folderPath);
            }
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            EditorGUI.BeginChangeCheck();

            targetFolder = EditorGUILayout.ObjectField("Folder", targetFolder, typeof(DefaultAsset), false) as DefaultAsset;

            if (EditorGUI.EndChangeCheck())
            {
                if (targetFolder != null)
                    site.folderPath = AssetDatabase.GetAssetPath(targetFolder);

                EditorUtility.SetDirty(target);
            }
        }
    }
}