using UnityEngine;

namespace Com2usGameDev
{
    public static class GameObjectEditorExtensions
    {
        public static void AsPrefab(this GameObject origin, string path, string name = "")
        {
            name = string.IsNullOrEmpty(name) ? origin.name : name;
            EditorToolset.CreatePrefab(origin, path, name);
        }
    }
}
