using System;
using UnityEngine;

namespace GameLogic
{
    [Serializable]
    public class AudioTrack
    {
        public string name;
        public AudioClip audioClip;
        [Space] [Range(0, 1)] public float volume;
        [Range(0.1f, 3)] public float pitch;
        public bool loop;

        [HideInInspector] public AudioSource source;

        public void SetSource(AudioSource source)
        {
            this.source = source;
            UpdateSource();
        }

        public void UpdateSource()
        {
            if (source == null) return;
            source.clip = audioClip;
            source.playOnAwake = false;
            source.volume = volume;
            source.pitch = pitch;
            source.loop = loop;
        }
    }
}