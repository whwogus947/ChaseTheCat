using UnityEditor;

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
                sonorous.audioChannel = EditorUtility.FindAudioChannel();
        }
    }
}