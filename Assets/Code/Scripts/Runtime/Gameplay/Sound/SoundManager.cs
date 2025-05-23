using System;
using UnityEngine;

namespace Com2usGameDev
{
    public class SoundManager : Manager
    {
        public AudioBroadcaster BGM;
        public AudioBroadcaster SFX;

        private void Start()
        {
            BGM.channel.AddEvent(PlayBGM);
            SFX.channel.AddEvent(PlaySFX);
            BGM.volumeHandler.onVolumeChange += BGM.ChangeVolume;
            SFX.volumeHandler.onVolumeChange += SFX.ChangeVolume;
        }

        private void OnDestroy()
        {
            BGM.channel.RemoveEvent(PlayBGM);
            SFX.channel.RemoveEvent(PlaySFX);
            BGM.volumeHandler.onVolumeChange -= BGM.ChangeVolume;
            SFX.volumeHandler.onVolumeChange -= SFX.ChangeVolume;
        }

        public void PlaySFX(AudioClip clip)
        {
            SFX.source.PlayOneShot(clip);
        }
        
        public void PlayBGM(AudioClip clip)
        {
            BGM.source.clip = clip;
            BGM.source.Play();
        }
    }

    [System.Serializable]
    public class AudioBroadcaster
    {
        public VolumeHandlerSO volumeHandler;
        public AudioChannelSO channel;
        public AudioSource source;

        public void ChangeVolume(float value)
        {
            source.volume = value;
        }
    }
}
