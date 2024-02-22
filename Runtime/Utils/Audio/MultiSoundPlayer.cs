using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

namespace SkalluUtils.Utils.Audio
{
    [RequireComponent(typeof(AudioSource))]
    public class MultiSoundPlayer : MonoBehaviour
    {
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private List<SoundClip> _sounds = new List<SoundClip>();
        [SerializeField, Min(1)] private int _playbackVoices = 6;
        [SerializeField, PropertyAttributes.ReadOnly] private int _currentVoice;
        [SerializeField] private bool _paused;

        private int[] _voiceClipIndex;
        private float[] _voiceTimer, _voicePitch, _voiceVolume;
        private bool[] _voicePlaying, _voiceStarted;
        private int _sampleRate;
        private bool _loaded;
        private bool _playingAnything;
       
        internal List<SoundClip> Sounds => _sounds;
        public int SoundsCount => _sounds.Count;

#if UNITY_EDITOR
        private void Reset()
        {
            if (_audioSource == null)
            {
                _audioSource = GetComponent<AudioSource>();
            }
        }
#endif

        private void Start()
        {
            _voiceClipIndex = new int[_playbackVoices];
            _voiceTimer = new float[_playbackVoices];
            _voicePitch = new float[_playbackVoices];
            _voiceVolume = new float[_playbackVoices];
            _voicePlaying = new bool[_playbackVoices];
            _voiceStarted = new bool[_playbackVoices];

            for (int i = 0; i < _playbackVoices; i++)
            {
                ResetVoice(i);
            }

            _sampleRate = AudioSettings.outputSampleRate;

            foreach (var soundClip in _sounds)
            {
                soundClip.LoadData(_sampleRate);
            }

            _loaded = true;
        }

        private void ResetVoice(int index)
        {
            _voiceClipIndex[index] = -1;
            _voiceTimer[index] = 0;
            _voiceVolume[index] = 0;
            _voicePitch[index] = 0;
            _voicePlaying[index] = false;
            _voiceStarted[index] = false;
        }

        private void Update()
        {
            _playingAnything = false;
        
            for (int i = 0; i <_playbackVoices; i++)
            {
                if (_voicePlaying[i])
                {
                    _playingAnything = true;
                }
            }

            if (_paused)
            {
                if (_audioSource.isPlaying && _playingAnything)
                {
                    _audioSource.Stop();
                }
            }
            else
            {
                if (_audioSource.isPlaying && !_playingAnything)
                {
                    _audioSource.Stop();
                }

                if (!_audioSource.isPlaying && _playingAnything)
                {
                    _audioSource.Play();
                }
            }
        }
    
        private void OnAudioFilterRead(float[] data, int channels)
        {
            if (!_loaded)
            {
                return;
            }

            if (_playingAnything)
            {
                for (int i = 0; i < data.Length; i++)
                {
                    data[i] = 0;
                }

                for (int v = 0; v < _playbackVoices; v++)
                {
                    if (_voicePlaying[v])
                    {
                        _voiceStarted[v] = true;
                    }
                }

                var invertedSampleRate = 1 / (float) _sampleRate;

                var channelData = new float[channels, data.Length / channels];
                var channelLength = data.Length / channels;

                for (int c = 0; c < channels; c++)
                {
                    for (int s = 0; s < channelLength; s++)
                    {
                        channelData[c, s] = 0;
                    }
                }

                for (int s = 0; s < data.Length / channels; s++)
                {
                    for (int v = 0; v < _playbackVoices; v++)
                    {
                        if (_voicePlaying[v] && _voiceStarted[v])
                        {
                            var sound = _sounds[_voiceClipIndex[v]];
                            
                            for (int c = 0; c < channels; c++)
                            {
                                var channelLimit = c;
                                
                                if (channelLimit >= sound.Channels)
                                {
                                    channelLimit = sound.Channels - 1;
                                }

                                var currentSample = (int) ((sound.Samples.Length / sound.Channels) * _voiceTimer[v]);

                                channelData[c, s] += sound.ChannelSamples[channelLimit, currentSample] * _voiceVolume[v];
                            }
                        }
                    }

                    for (int v = 0; v < _playbackVoices; v++)
                    {
                        if (_voicePlaying[v] && _voiceStarted[v])
                        {
                            _voiceTimer[v] += _voicePitch[v] * invertedSampleRate / _sounds[_voiceClipIndex[v]].Length *
                                              _sounds[_voiceClipIndex[v]].PlayBackRate;
                        }

                        if (_voiceTimer[v] >= 1)
                        {
                            _voicePlaying[v] = false;
                            _voiceStarted[v] = false;
                        }
                    }
                }

                for (int c = 0; c < channels; c++)
                {
                    for (int s = 0; s < channelLength; s++)
                    {
                        data[s * channels + c] = channelData[c, s];
                    }
                }
            }
            else
            {
                for (int i = 0; i < data.Length; i++)
                {
                    data[i] = 0;
                }
            }
        }

        private void PlaySingleSoundInternal(int soundIndex)
        {
            if (_currentVoice >= _playbackVoices)
            {
                _currentVoice = 0;
            }

            if (_voicePlaying[_currentVoice])
            {
                return;
            }

            var sound = _sounds[soundIndex];
            _voiceClipIndex[_currentVoice] = soundIndex;
            _voicePitch[_currentVoice] = sound.Pitch + Random.Range(-sound.PitchRandomRange, sound.PitchRandomRange);
            _voiceVolume[_currentVoice] = sound.Volume + Random.Range(-sound.VolumeRandomRange, sound.VolumeRandomRange);
            _voiceTimer[_currentVoice] = 0;
            _voicePlaying[_currentVoice] = true;

            _currentVoice += 1;
        }
        
        /// <summary>
        /// Plays sound at index
        /// </summary>
        /// <param name="soundIndex"> sound index to play </param>
        public void PlaySingleSound(int soundIndex)
        {
            if (soundIndex >= _sounds.Count || _sounds[soundIndex] == null)
            {
                Debug.LogError($"Sound with id {soundIndex} does not exist!");
                return;
            }

            PlaySingleSoundInternal(soundIndex);
        }
        
        /// <summary>
        /// Plays sound by finding it's index
        /// </summary>
        /// <param name="soundClip"> sound clip to play </param>
        public void PlaySingleSound([NotNull] SoundClip soundClip)
        {
            if (_sounds.Contains(soundClip))
            {
                PlaySingleSoundInternal(_sounds.IndexOf(soundClip));
            }
            else
            {
                Debug.LogError("provided sound clip is not inside sounds list!");
            }
        }
        
        /// <summary>
        /// Pauses/Unpauses
        /// </summary>
        /// <param name="value"> true - pause, false - unpause </param>
        public void Pause(bool value)
        {
            _paused = value;
        }

        /// <summary>
        /// Stops all currently playing sounds
        /// </summary>
        public void Stop()
        {
            for (int i = 0; i < _playbackVoices; i++)
            {
                if (_voicePlaying[i])
                {
                    _voicePlaying[i] = false;
                    _voiceStarted[i] = false;
                    ResetVoice(i);
                }
            }
        }
        
        /// <summary>
        /// Stops the sound at a specific index if it's playing.
        /// </summary>
        /// <param name="soundIndex"> index of the sound to stop </param>
        public void StopSoundAtIndex(int soundIndex)
        {
            for (int i = 0; i < _playbackVoices; i++)
            {
                if (_voicePlaying[i] && _voiceClipIndex[i] == soundIndex)
                {
                    _voicePlaying[i] = false;
                    _voiceStarted[i] = false;
                    ResetVoice(i);
                    break;
                }
            }
        }
    }
}