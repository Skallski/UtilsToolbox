using System;
using UnityEngine;

namespace SkalluUtils.Utils
{
    namespace Sound
    {
        [Serializable]
        public struct Sound
        {
            [SerializeField] internal AudioClip audioClip;
        
            [SerializeField] [Range(0,1)] internal float volume;
            [SerializeField] [Range(0,1)] internal float pitch;
        }

        public static class SoundUtils
        {
            /// <summary>
            /// Plays single sound from audio source
            /// </summary>
            /// <param name="audioSource"> audio source on which sound will be played </param>
            /// <param name="sound"> sound to play </param>
            public static void PlaySingleSound(AudioSource audioSource, Sound sound)
            {
                audioSource.volume = sound.volume;
                audioSource.pitch = sound.pitch;
        
                audioSource.PlayOneShot(sound.audioClip);
            }

            /// <summary>
            /// Plays selected audio source
            /// </summary>
            /// <param name="audioSource"> audio source to play </param>
            public static void Play(AudioSource audioSource)
            {
                if (!audioSource.isPlaying)
                    audioSource.Play();
            }

            /// <summary>
            /// Pauses selected audio source
            /// </summary>
            /// <param name="audioSource"> audio source to pause </param>
            public static void Pause(AudioSource audioSource)
            {
                if (audioSource.isPlaying)
                    audioSource.Pause();
            }
            
        } 
    }
}