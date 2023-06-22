using System;
using UnityEngine;

namespace SkalluUtils.Utils.MultiSoundPlayer
{
    [Serializable]
    public class SoundClip
    {
        [SerializeField] private AudioClip _audioClip;
        [SerializeField, Range(0, 1)] private float _volume = 1;
        [SerializeField, Range(-3, 3)] private float _pitch = 1;
        [Space]
        [SerializeField, Range(0, 1)] private float _volumeRandomRange;
        [SerializeField, Range(0, 1)] private float _pitchRandomRange;

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

        // public SoundClip() {}
        //
        // public SoundClip(AudioClip audioClip, float volume, float pitch, float volumeRandomRange = 0, float pitchRandomRange = 0)
        // {
        //     _audioClip = audioClip;
        //     _volume = volume;
        //     _pitch = pitch;
        //     _volumeRandomRange = volumeRandomRange;
        //     _pitchRandomRange = pitchRandomRange;
        // }
    }
}