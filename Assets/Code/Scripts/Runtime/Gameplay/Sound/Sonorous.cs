using UnityEngine;

namespace Com2usGameDev
{
    public class Sonorous : MonoBehaviour
    {
        [ReadOnly]
        public AudioChannelSO audioChannel;

        public void PlaySound(AudioClip clip)
        {
            if (clip != null)
                audioChannel.Invoke(clip);
        }
    }
}
