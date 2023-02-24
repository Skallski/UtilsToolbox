using System.Collections.Generic;
using SkalluUtils.PropertyAttributes;
using UnityEngine;
using Random = UnityEngine.Random;

namespace SkalluUtils.Utils
{
    [RequireComponent(typeof(AudioSource))]
    public class MultiSoundPlayer : MonoBehaviour
    {
        #region INSPECTOR FIELDS
        [SerializeField] private AudioSource _audioSource;
        [Space]
        [SerializeField] private List<SoundClip> _sounds = new List<SoundClip>();
        [Space]
        [SerializeField] private bool _pause;
        [SerializeField] private int _playbackVoices = 6;
        [SerializeField, ReadOnly] private int _currentVoice = 0;
        #endregion

        #region NON SERIALIZED FIELDS
        private int[] _voiceClipIndex;
        private float[] _voiceTimer, _voicePitch, _voiceVolume;
        private bool[] _voicePlaying, _voiceStarted;
        private int _sampleRate;
        private bool _loaded = false;
        private bool _playingAnything = false;
        private readonly List<int> _indexesToPlay = new List<int>();
        #endregion
        
        #region PROPERTIES
        public AudioSource AudioSource
        {
            get => _audioSource;
            set => _audioSource = value;
        }

        public List<SoundClip> Sounds
        {
            get => _sounds;
            set => _sounds = value;
        }
        
        public bool Pause
        {
            get => _pause;
            set => _pause = value;
        }
        #endregion

        private void Awake()
        {
            if (AudioSource == null)
                AudioSource = GetComponent<AudioSource>();
        }

        private void Start()
        {
            _voiceClipIndex = new int[_playbackVoices];
            _voiceTimer = new float[_playbackVoices];
            _voicePitch = new float[_playbackVoices];
            _voiceVolume = new float[_playbackVoices];
            _voicePlaying = new bool[_playbackVoices];
            _voiceStarted = new bool[_playbackVoices];

            for (var i = 0; i < _playbackVoices; i++)
            {
                _voiceClipIndex[i] = -1;
                _voiceTimer[i] = 0;
                _voiceVolume[i] = 0;
                _voicePitch[i] = 0;
                _voicePlaying[i] = false;
                _voiceStarted[i] = false;
            }

            _sampleRate = AudioSettings.outputSampleRate;

            foreach (var t in _sounds)
            {
                t.Samples = new float[t.AudioClip.samples*t.AudioClip.channels];
                t.AudioClip.GetData(t.Samples, 0);
                t.Channels = t.AudioClip.channels;

                t.ChannelSamples = new float[t.Channels, (t.Samples.Length / t.Channels)];
                for (var c = 0; c < t.Channels; c++)
                {
                    for (var s = 0; s < t.Samples.Length / t.Channels; s++)
                    {
                        t.ChannelSamples[c, s] = t.Samples[s * t.Channels + c];
                    }
                }

                t.SampleRate = t.AudioClip.frequency;
                t.Length = t.AudioClip.length;

                t.PlayBackRate = (float)t.SampleRate / (float)_sampleRate;
            }

            _loaded = true;
        }

        private void Update()
        {
            _playingAnything = false;
        
            for (var i=0; i<_playbackVoices; i++)
            {
                if (_voicePlaying[i])
                    _playingAnything = true;
            }

            if (Pause == false)
            {
                if (AudioSource.isPlaying && _playingAnything == false)
                    AudioSource.Stop();

                if (!AudioSource.isPlaying && _playingAnything)
                    AudioSource.Play();
            }
            else
            {
                if(AudioSource.isPlaying && _playingAnything == true)
                    AudioSource.Stop();
            }

            if (_indexesToPlay.Count > 0)
            {
                foreach (var t in _indexesToPlay)
                {
                    PlaySingleSound(t);
                }

                _indexesToPlay.Clear();
            }
        }
    
        private void OnAudioFilterRead(float[] data, int channels)
        {
            if (_loaded)
            {
                if (_playingAnything)
                {
                    for (var i = 0; i < data.Length; i++)
                    {
                        data[i] = 0;
                    }

                    for (var v = 0; v < _playbackVoices; v++)
                    {
                        if (_voicePlaying[v])
                            _voiceStarted[v] = true;
                    }

                    var inv = 1 / (float) _sampleRate;

                    var channelData = new float[channels, data.Length / channels];
                    var channelLength = data.Length / channels;

                    for (var c = 0; c < channels; c++)
                    {
                        for (var s = 0; s < channelLength; s++)
                        {
                            channelData[c, s] = 0;
                        }
                    }

                    for (var s = 0; s < (data.Length / channels); s++)
                    {
                        for (var v = 0; v < _playbackVoices; v++)
                        {
                            if (_voicePlaying[v] == true && _voiceStarted[v] == true)
                            {
                                for (var c = 0; c < channels; c++)
                                {
                                    var channelLimit = c;
                                
                                    if (channelLimit >= _sounds[_voiceClipIndex[v]].Channels)
                                        channelLimit = _sounds[_voiceClipIndex[v]].Channels - 1;

                                    var currentSample = (int) ((_sounds[_voiceClipIndex[v]].Samples.Length / _sounds[_voiceClipIndex[v]].Channels) * _voiceTimer[v]);

                                    channelData[c, s] += _sounds[_voiceClipIndex[v]].ChannelSamples[channelLimit, currentSample] * _voiceVolume[v];
                                }
                            }
                        }

                        for (var v = 0; v < _playbackVoices; v++)
                        {
                            if (_voicePlaying[v] && _voiceStarted[v])
                                _voiceTimer[v] += _voicePitch[v] * inv / _sounds[_voiceClipIndex[v]].Length * _sounds[_voiceClipIndex[v]].PlayBackRate;

                            if (_voiceTimer[v] >= 1)
                            {
                                _voicePlaying[v] = false;
                                _voiceStarted[v] = false;
                            }
                        }
                    }

                    for (var c = 0; c < channels; c++)
                    {
                        for (var s = 0; s < channelLength; s++)
                        {
                            data[s * channels + c] = channelData[c, s]; ;
                        }
                    }
                }
                else
                {
                    for (var i=0; i<data.Length; i++)
                    {
                        data[i] = 0;
                    }
                }
            }
        }

        public void PlaySingleSound(int soundIndex)
        {
            if (_currentVoice >= _playbackVoices)
                _currentVoice = 0;

            if (_voicePlaying[_currentVoice] == false && _voicePlaying[_currentVoice] == false)
            {
                _voiceClipIndex[_currentVoice] = soundIndex;
                _voicePitch[_currentVoice] = _sounds[soundIndex].Pitch + Random.Range(-_sounds[soundIndex].PitchRandomRange, _sounds[soundIndex].PitchRandomRange);
                _voiceVolume[_currentVoice] = _sounds[soundIndex].Volume + Random.Range(-_sounds[soundIndex].VolumeRandomRange, _sounds[soundIndex].VolumeRandomRange);
                _voiceTimer[_currentVoice] = 0;
                _voicePlaying[_currentVoice] = true;

                _currentVoice += 1;
            }
        }

        public void PlaySingleSoundSafe(int soundIndex)
        {
            if (_loaded)
                PlaySingleSound(soundIndex);
            else
                _indexesToPlay.Add(soundIndex);
        }

    }
}