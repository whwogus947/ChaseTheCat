using UnityEngine;
using UnityEditor;

namespace Com2usGameDev
{
    public static class EditorUtility
    {
        public static AudioChannelSO FindAudioChannel()
        {
            string typeName = nameof(AudioChannelSO);
            string[] guids = AssetDatabase.FindAssets($"SFX Channel t:{typeName}");
            foreach (string guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                AudioChannelSO asset = AssetDatabase.LoadAssetAtPath<AudioChannelSO>(path);
                if (asset != null)
                {
                    return asset;
                }
            }
            return null;
        }
    }
}
