using System.Collections;
using System.Collections.Generic;
using Bap.DependencyInjection;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Pool;
using UnityEngine.UI;
using Utils;

namespace Manager
{
    public class SoundManager : Singleton<SoundManager>
    {
        [SerializeField] private AudioSource audioSourcePref;
        [SerializeField] private AudioMixer audioMixer;
        [SerializeField] private Slider musicSlider;
        [SerializeField] private Slider sfxSlider;
        [SerializeField, Tooltip("Audio Clips play in background")] private List<AudioClip> audioClips;
        [SerializeField] private AudioSource soundBackground;

        private ObjectPool<AudioSource> audioSourcePool;

        [Provide] private SoundManager Construct() => this;

        private void Start()
        {
            audioSourcePool =
                new ObjectPool<AudioSource>(CreateNewAudioSource, GetAS, ReleaseAS, DestroyAS, false, 10, 20);

            soundBackground.clip = audioClips[Random.Range(0, audioClips.Count - 1)];
            soundBackground?.Play();

            audioMixer.SetFloat("music", 1);
            audioMixer.SetFloat("sfx", 1);
        }

        private void DestroyAS(AudioSource obj)
        {
            Destroy(obj.gameObject);
        }

        private void ReleaseAS(AudioSource obj)
        {
            obj.gameObject.SetActive(false);
        }

        private void GetAS(AudioSource obj)
        {
            obj.gameObject.SetActive(true);
        }

        private AudioSource CreateNewAudioSource()
        {
            AudioSource audioSource = Instantiate(audioSourcePref);
            audioSource.gameObject.SetActive(false);
            
            return audioSource;
        }

        public void StopBackgroundMusic()
        {
            soundBackground?.Stop();
        }

        public void ResumeBackgroundMusic()
        {
            soundBackground?.Play();
        }

        public void onMusicSliderValueChanged()
        {
            float value = musicSlider.value;
            audioMixer.SetFloat("music", Mathf.Log10(value)*20);
        }
        
        public void onSFXSliderValueChanged()
        {
            float value = sfxSlider.value;
            audioMixer.SetFloat("sfx", Mathf.Log10(value)*20);
        }

        public void PlaySFX(AudioClip audioClip, float volume)
        {
            AudioSource audioSource = audioSourcePool.Get();
            audioSource.outputAudioMixerGroup = audioMixer.FindMatchingGroups("Master")[2];
            audioSource.clip = audioClip;
            StartCoroutine(ReleaseAudioSourceToPoolAfter(audioSource, audioSource.clip.length));
        }

        private IEnumerator ReleaseAudioSourceToPoolAfter(AudioSource audiorSource, float clipLength)
        {
            yield return new WaitForSeconds(clipLength);
            audioSourcePool.Release(audiorSource);
        }
    }
}
