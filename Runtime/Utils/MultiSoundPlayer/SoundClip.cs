using System;
using UnityEngine;

namespace SkalluUtils.Utils
{
    [Serializable]
    public class SoundClip
    {
        #region INSPECTOR FIELDS
        [SerializeField] private AudioClip _audioClip;
        [Space]
        [SerializeField, Range(0, 1)] private float _volume = 1;
        [SerializeField, Range(0, 1)] private float _volumeRandomRange;
        [Space]
        [SerializeField, Range(-3, 3)] private float _pitch = 1;
        [SerializeField, Range(0, 1)] private float _pitchRandomRange;
        #endregion

        public AudioClip AudioClip => _audioClip;
        public float Volume => _volume;
        public float VolumeRandomRange => _volumeRandomRange;
        public float Pitch => _pitch;
        public float PitchRandomRange => _pitchRandomRange;

        public float[] Samples { get; set; }
        public float[,] ChannelSamples { get; set; }
        public int Channels { get; set; }
        public float Length { get; set; }
        public int SampleRate { get; set; }
        public float PlayBackRate { get; set; }

        public SoundClip(AudioClip audioClip, float volume, float volumeRandomRange, float pitch, float pitchRandomRange)
        {
            _audioClip = audioClip;
            _volume = volume;
            _volumeRandomRange = volumeRandomRange;
            _pitch = pitch;
            _pitchRandomRange = pitchRandomRange;
        }
    }
}