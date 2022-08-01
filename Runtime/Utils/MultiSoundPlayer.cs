using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace SkalluUtils.Utils
{
    [RequireComponent(typeof(AudioSource))]
    public class MultiSoundPlayer : MonoBehaviour
    {
        public List<SoundClip> sounds = new List<SoundClip>();
        public AudioSource audioSource;
        public bool pause;

        [Serializable]
        public class SoundClip
        {
            [SerializeField] internal AudioClip audioClip;
            [SerializeField] internal float volume;
            [SerializeField] internal float volumeRandomRange;
            [SerializeField] internal float pitch;
            [SerializeField] internal float pitchRandomRange;

            internal float[] samples;
            internal float[,] channelSamples;
            internal int channels;
            internal float lenght;
            internal int samplerate;
            internal float playBackRate;
        }

        public int playbackVoices = 6;
        public int currentVoice = 0;

        private int[] voice_clipIndex;
        private float[] voice_timer;
        private float[] voice_pitch;
        private float[] voice_volume;
        private bool[] voice_playing;
        private bool[] voice_started;

        private int sampleRate;

        public bool restartVoices = true;
        private bool loaded = false;
        private bool playingAnything = false;

        private readonly List<int> indexesToPlay = new List<int>();

        private void Awake() => audioSource ??= GetComponent<AudioSource>();

        private void Start()
        {
            voice_clipIndex = new int[playbackVoices];
            voice_timer = new float[playbackVoices];
            voice_pitch = new float[playbackVoices];
            voice_volume = new float[playbackVoices];
            voice_playing = new bool[playbackVoices];
            voice_started = new bool[playbackVoices];

            for (int i = 0; i < playbackVoices; i++)
            {
                voice_clipIndex[i] = -1;
                voice_timer[i] = 0;
                voice_volume[i] = 0;
                voice_pitch[i] = 0;
                voice_playing[i] = false;
                voice_started[i] = false;
            }

            sampleRate = AudioSettings.outputSampleRate;

            for (int i = 0; i < sounds.Count; i++)
            {
                sounds[i].samples = new float[sounds[i].audioClip.samples*sounds[i].audioClip.channels];

                sounds[i].audioClip.GetData(sounds[i].samples, 0);

                sounds[i].channels = sounds[i].audioClip.channels;

                sounds[i].channelSamples = new float[sounds[i].channels, (sounds[i].samples.Length / sounds[i].channels)];
                for (int c = 0; c < sounds[i].channels; c++)
                {
                    for (int s = 0; s < sounds[i].samples.Length / sounds[i].channels; s++)
                    {
                        sounds[i].channelSamples[c, s] = sounds[i].samples[s * sounds[i].channels + c];
                    }
                }

                sounds[i].samplerate = sounds[i].audioClip.frequency;
                sounds[i].lenght = sounds[i].audioClip.length;

                sounds[i].playBackRate = (float)sounds[i].samplerate / (float)sampleRate;
            }

            loaded = true;
        }

        private void Update()
        {
            playingAnything = false;
            
            for (int i=0; i<playbackVoices; i++)
            {
                if (voice_playing[i])
                    playingAnything = true;
            }

            if (pause == false)
            {
                if (audioSource.isPlaying && playingAnything == false)
                    audioSource.Stop();

                if (!audioSource.isPlaying && playingAnything)
                    audioSource.Play();
            }
            else
            {
                if(audioSource.isPlaying && playingAnything == true)
                    audioSource.Stop();
            }

            if (indexesToPlay.Count > 0)
            {
                foreach (var t in indexesToPlay)
                {
                    PlaySingleSound(t);
                }

                indexesToPlay.Clear();
            }
        }
        
        private void OnAudioFilterRead(float[] data, int channels)
        {
            if (loaded)
            {
                if (playingAnything)
                {
                    for (int i = 0; i < data.Length; i++)
                    {
                        data[i] = 0;
                    }

                    for (int v = 0; v < playbackVoices; v++)
                    {
                        if (voice_playing[v])
                        {
                            voice_started[v] = true;
                        }
                    }

                    float inv = 1 / (float) sampleRate;

                    float[,] channelData = new float[channels, data.Length / channels];
                    int channelLenght = data.Length / channels;

                    for (int c = 0; c < channels; c++)
                    {
                        for (int s = 0; s < channelLenght; s++)
                        {
                            channelData[c, s] = 0;
                        }
                    }


                    for (int s = 0; s < (data.Length / channels); s++)
                    {

                        for (int v = 0; v < playbackVoices; v++)
                        {
                            if (voice_playing[v] == true && voice_started[v] == true)
                            {
                                for (int c = 0; c < channels; c++)
                                {
                                    int channelLimit = c;
                                    
                                    if (channelLimit >= sounds[voice_clipIndex[v]].channels)
                                        channelLimit = sounds[voice_clipIndex[v]].channels - 1;

                                    int currentSample = (int)((sounds[voice_clipIndex[v]].samples.Length / sounds[voice_clipIndex[v]].channels) * voice_timer[v]);

                                    channelData[c, s] += sounds[voice_clipIndex[v]].channelSamples[channelLimit, currentSample] * voice_volume[v];
                                }
                            }
                        }

                        for (int v = 0; v < playbackVoices; v++)
                        {
                            if (voice_playing[v] && voice_started[v])
                                voice_timer[v] += voice_pitch[v] * inv / sounds[voice_clipIndex[v]].lenght * sounds[voice_clipIndex[v]].playBackRate;

                            if (voice_timer[v] >= 1)
                            {
                                voice_playing[v] = false;
                                voice_started[v] = false;
                            }
                        }
                    }

                    for (int c = 0; c < channels; c++)
                    {
                        for (int s = 0; s < channelLenght; s++)
                        {
                            data[s * channels + c] = channelData[c, s]; ;
                        }
                    }
                }
                else
                {
                    for (int i=0; i<data.Length; i++)
                    {
                        data[i] = 0;
                    }
                }
            }
        }

        public void PlaySingleSound(int soundIndex)
        {
            if (currentVoice >= playbackVoices)
                currentVoice = 0;

            if (voice_playing[currentVoice] == false && voice_playing[currentVoice] == false)
            {
                voice_clipIndex[currentVoice] = soundIndex;
                voice_pitch[currentVoice] = sounds[soundIndex].pitch + Random.Range(-sounds[soundIndex].pitchRandomRange, sounds[soundIndex].pitchRandomRange);
                voice_volume[currentVoice] = sounds[soundIndex].volume + Random.Range(-sounds[soundIndex].volumeRandomRange, sounds[soundIndex].volumeRandomRange);
                voice_timer[currentVoice] = 0;
                voice_playing[currentVoice] = true;

                currentVoice += 1;
            }
        }

        public void PlaySingleSoundSafe(int soundIndex)
        {
            if (loaded)
                PlaySingleSound(soundIndex);
            else
                indexesToPlay.Add(soundIndex);
        }
    }
}