using System.Collections.Generic;
using UnityEngine;
using Utils;

namespace Manager
{
    public class SoundManager : Singleton<SoundManager>
    {
        [Tooltip("Audio Clips play in background")]
        public List<AudioClip> audioClips;
        public AudioSource soundBackground;

        private void Start() {
            soundBackground.clip = audioClips[Random.Range(0, audioClips.Count-1)];
            soundBackground?.Play();
        }
    }
}
