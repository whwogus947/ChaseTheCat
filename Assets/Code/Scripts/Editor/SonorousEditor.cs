using UnityEditor;
using UnityEngine;

namespace Com2usGameDev
{
    [CustomEditor(typeof(Sonorous), true)]
    public class SonorousEditor : Editor
    {
        private Sonorous sonorous;

        private void OnEnable()
        {
            sonorous = (Sonorous)target;
            if (sonorous.audioChannel == null)
                FindAudioChannel();
        }

        private void FindAudioChannel()
        {
            string typeName = nameof(AudioChannelSO);
            string[] guids = AssetDatabase.FindAssets($"SFX Channel t:{typeName}");
            foreach (string guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                AudioChannelSO asset = AssetDatabase.LoadAssetAtPath<AudioChannelSO>(path);
                if (asset != null)
                {
                    sonorous.audioChannel = asset;
                }
            }
        }
    }
}
