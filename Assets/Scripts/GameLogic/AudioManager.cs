using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GameLogic
{
    public class AudioManager : MonoBehaviour
    {
        [SerializeField] private List<AudioTrack> tracks = new();
        [SerializeField] private string playOnStart;
        private static AudioManager instance;

        public void Awake()
        {
            DontDestroyOnLoad(gameObject);
            if (instance == null)
                instance = this;
            else
            {
                Debug.LogWarning("Destroyed Singleton Object: " + name);
                Destroy(this);
            }
        }

        public static void PlayAudioTrack(string name)
        {
            if (instance == null || !instance.TryPlayAudio(name))
                Debug.LogWarning($"AudioTrack {name} not found");
        }

        public static bool HasTrack(string name) => instance == null || instance.tracks.Any(t => t.name == name);

        public static void ValidateTrack(string name, Object obj)
        {
            if (!HasTrack(name))
                Debug.LogWarning($"AudioTrack {name} not found{(obj != null ? $" on object {obj.name}" : "")}");
        }

        private bool TryPlayAudio(string name)
        {
            var track = tracks.Find(t => t.name == name);
            if (track == null) return false;
            if (track.source == null)
                track.SetSource(gameObject.AddComponent<AudioSource>());
            track.source.Play();
            return true;
        }

        private void Start()
        {
            if (playOnStart is {Length: > 0})
                PlayAudioTrack(playOnStart);
        }

        private void OnValidate()
        {
            tracks.ForEach(t => t.UpdateSource());
        }
    }
}