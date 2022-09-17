using System;
using System.Collections.Generic;
using SkalluUtils.PropertyAttributes;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(AudioSource))]
public class MultiSoundPlayer : MonoBehaviour
{
    [Serializable]
    public class SoundClip
    {
        public AudioClip audioClip;
    
        [Space]
        [Range(0, 1)] public float volume;
        [Range(0, 1)] public float volumeRandomRange;
    
        [Space]
        [Range(-3, 3)] public float pitch;
        [Range(0, 1)] public float pitchRandomRange;
    
        internal float[] samples;
        internal float[,] channelSamples;
        internal int channels;
        internal float lenght;
        internal int samplerate;
        internal float playBackRate;
    }
    
    #region INSPECTOR FIELDS
    public AudioSource audioSource;
    
    [Space]
    public List<SoundClip> sounds = new List<SoundClip>();

    [Space]
    public bool pause;
    [SerializeField] private int playbackVoices = 6;
    [SerializeField, ReadOnlyInspector] private int currentVoice = 0;
    #endregion

    private int[] voice_clipIndex;
    private float[] voice_timer, voice_pitch, voice_volume;
    private bool[] voice_playing, voice_started;

    private int sampleRate;
    
    private bool loaded = false;
    private bool playingAnything = false;

    private readonly List<int> indexesToPlay = new List<int>();

    private void Awake()
    {
        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        voice_clipIndex = new int[playbackVoices];
        voice_timer = new float[playbackVoices];
        voice_pitch = new float[playbackVoices];
        voice_volume = new float[playbackVoices];
        voice_playing = new bool[playbackVoices];
        voice_started = new bool[playbackVoices];

        for (var i = 0; i < playbackVoices; i++)
        {
            voice_clipIndex[i] = -1;
            voice_timer[i] = 0;
            voice_volume[i] = 0;
            voice_pitch[i] = 0;
            voice_playing[i] = false;
            voice_started[i] = false;
        }

        sampleRate = AudioSettings.outputSampleRate;

        foreach (var t in sounds)
        {
            t.samples = new float[t.audioClip.samples*t.audioClip.channels];
            t.audioClip.GetData(t.samples, 0);
            t.channels = t.audioClip.channels;

            t.channelSamples = new float[t.channels, (t.samples.Length / t.channels)];
            for (var c = 0; c < t.channels; c++)
            {
                for (var s = 0; s < t.samples.Length / t.channels; s++)
                {
                    t.channelSamples[c, s] = t.samples[s * t.channels + c];
                }
            }

            t.samplerate = t.audioClip.frequency;
            t.lenght = t.audioClip.length;

            t.playBackRate = (float)t.samplerate / (float)sampleRate;
        }

        loaded = true;
    }

    private void Update()
    {
        playingAnything = false;
        
        for (var i=0; i<playbackVoices; i++)
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
                for (var i = 0; i < data.Length; i++)
                {
                    data[i] = 0;
                }

                for (var v = 0; v < playbackVoices; v++)
                {
                    if (voice_playing[v])
                        voice_started[v] = true;
                }

                var inv = 1 / (float) sampleRate;

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
                    for (var v = 0; v < playbackVoices; v++)
                    {
                        if (voice_playing[v] == true && voice_started[v] == true)
                        {
                            for (var c = 0; c < channels; c++)
                            {
                                var channelLimit = c;
                                
                                if (channelLimit >= sounds[voice_clipIndex[v]].channels)
                                    channelLimit = sounds[voice_clipIndex[v]].channels - 1;

                                var currentSample = (int) ((sounds[voice_clipIndex[v]].samples.Length / sounds[voice_clipIndex[v]].channels) * voice_timer[v]);

                                channelData[c, s] += sounds[voice_clipIndex[v]].channelSamples[channelLimit, currentSample] * voice_volume[v];
                            }
                        }
                    }

                    for (var v = 0; v < playbackVoices; v++)
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
