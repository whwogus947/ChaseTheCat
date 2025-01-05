using UnityEngine;
using UnityEditor;
using System.IO;

namespace Com2usGameDev
{
    public class SystemLauncherEditor
    {
        [MenuItem("Assets/Create/Cum2usGameDev/SystemLauncher")]
        public static void CreateMyScriptableObject()
        {
            string selectedPath = GetSelectedPath();

            if (!selectedPath.Contains("/Resources"))
            {
                UnityEditor.EditorUtility.DisplayDialog("Invalid Location",
                    "System launcher must be created inside a Resources folder.", "OK");
                return;
            }

            SystemLauncherSO asset = ScriptableObject.CreateInstance<SystemLauncherSO>();

            string assetPathAndName = AssetDatabase.GenerateUniqueAssetPath(
                selectedPath + "/New System Launcher.asset");

            AssetDatabase.CreateAsset(asset, assetPathAndName);
            AssetDatabase.SaveAssets();

            Selection.activeObject = asset;
        }

        private static string GetSelectedPath()
        {
            string path = AssetDatabase.GetAssetPath(Selection.activeObject);
            if (string.IsNullOrEmpty(path))
            {
                path = "Assets";
            }
            else if (Path.GetExtension(path) != "")
            {
                path = path.Replace(Path.GetFileName(path), "");
            }
            return path;
        }
    }
}
