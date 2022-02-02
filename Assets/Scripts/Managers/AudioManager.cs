using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BettingApp.Manager
{
    public class AudioManager : Singleton<AudioManager>
    {
        private AudioSource _audioSource;

        public AudioClip[] clips;

        private void Start()
        {
            _audioSource = this.GetComponent<AudioSource>();
        }

        /// <summary>
        /// Check if the FX sounds are enabled in the game settings.
        /// If so, play the proper audio clip.
        /// </summary>
        public void PlaySoundClip(int index)
        {
            if (GameManager.Instance.settings.enableFXSounds == EnableFXSounds.No)
                return;

            _audioSource.clip = clips[index];
            _audioSource.Play();
        }
    }
}