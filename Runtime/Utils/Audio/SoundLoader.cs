using System.Collections.Generic;
using UnityEngine;

namespace UtilsToolbox.Utils.Audio
{
    public static class SoundLoader
    {
        /// <summary>
        /// Clears list of MultiSoundPlayer's sound clips
        /// </summary>
        /// <param name="multiSoundPlayer"></param>
        public static void ClearSounds(this MultiSoundPlayer multiSoundPlayer)
        {
            multiSoundPlayer.Sounds.Clear();
        }

        /// <summary>
        /// Loads list of sound clips into the MultiSoundPlayer
        /// </summary>
        /// <param name="multiSoundPlayer"> multi sound player on which the method will be called </param>
        /// <param name="soundsClips"> list of sound clips to load </param>
        public static void LoadSounds(this MultiSoundPlayer multiSoundPlayer, List<SoundClip> soundsClips)
        {
            ClearSounds(multiSoundPlayer);
            AddSounds(multiSoundPlayer, soundsClips);
        }
        
        /// <summary>
        /// Loads sound clips into the MultiSoundPlayer
        /// </summary>
        /// <param name="multiSoundPlayer"> multi sound player on which the method will be called </param>
        /// <param name="soundsClips"> sound clips to load </param>
        public static void LoadSounds(this MultiSoundPlayer multiSoundPlayer, params SoundClip[] soundsClips)
        {
            ClearSounds(multiSoundPlayer);
            AddSounds(multiSoundPlayer, soundsClips);
        }

        /// <summary>
        /// Adds list of sounds clips into the MultiSoundPlayer
        /// </summary>
        /// <param name="multiSoundPlayer"></param>
        /// <param name="soundsClips"></param>
        public static void AddSounds(this MultiSoundPlayer multiSoundPlayer, List<SoundClip> soundsClips)
        {
            if (soundsClips == null || soundsClips.Count == 0)
            {
                Debug.LogError($"Cannot load sounds, because sound list is null or empty!");
                return;
            }

            if (multiSoundPlayer == null)
            {
                Debug.LogError($"Cannot load sounds, because MultiSoundPlayer is null!");
                return;
            }
            
            multiSoundPlayer.Sounds.AddRange(soundsClips);
        }

        /// <summary>
        /// Adds sound clipt into the MultiSoundPlayer
        /// </summary>
        /// <param name="multiSoundPlayer"></param>
        /// <param name="soundsClips"></param>
        public static void AddSounds(this MultiSoundPlayer multiSoundPlayer, params SoundClip[] soundsClips)
        {
            if (soundsClips == null || soundsClips.Length == 0)
            {
                Debug.LogError($"Cannot load sounds, because sound list is null or empty!");
                return;
            }

            if (multiSoundPlayer == null)
            {
                Debug.LogError($"Cannot load sounds, because MultiSoundPlayer is null!");
                return;
            }
            
            multiSoundPlayer.Sounds.AddRange(soundsClips);
        }
    }
}