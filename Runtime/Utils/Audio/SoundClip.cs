using UnityEngine;

namespace UtilsToolbox.Utils.Audio
{
    [System.Serializable]
    public class SoundClip
    {
        [SerializeField] private AudioClip _audioClip;
        [SerializeField, Range(0, 1)] private float _volume = 1;
        [SerializeField, Range(-3, 3)] private float _pitch = 1;
        [SerializeField, Range(0, 1)] private float _volumeRandomRange = 0;
        [SerializeField, Range(0, 1)] private float _pitchRandomRange = 0;

        public AudioClip AudioClip => _audioClip;
        public float Volume => _volume;
        public float VolumeRandomRange => _volumeRandomRange;
        public float Pitch => _pitch;
        public float PitchRandomRange => _pitchRandomRange;

        internal float[] Samples { get; private set; }
        internal float[,] ChannelSamples { get; private set; }
        internal int Channels { get; private set; }
        internal float Length { get; private set; }
        internal int SampleRate { get; private set; }
        internal float PlayBackRate { get; private set; }

        internal void LoadData(int sampleRate)
        {
            if (AudioClip == null)
            {
                return;
            }

            Samples = new float[AudioClip.samples * AudioClip.channels];
            AudioClip.GetData(Samples, 0);
            Channels = AudioClip.channels;
            ChannelSamples = new float[Channels, (Samples.Length / Channels)];

            for (var c = 0; c < Channels; c++)
            {
                for (var s = 0; s < Samples.Length / Channels; s++)
                {
                    ChannelSamples[c, s] = Samples[s * Channels + c];
                }
            }

            SampleRate = AudioClip.frequency;
            Length = AudioClip.length;
            PlayBackRate = SampleRate / (float) sampleRate;
        }
    }
}