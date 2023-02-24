using JetBrains.Annotations;
using SkalluUtils.PropertyAttributes;
using UnityEngine;
using UnityEngine.Audio;

namespace SkalluUtils.Utils
{
    public enum AudioType
    {
        Sfx,
        Music
    }

    public class VolumeManager : MonoBehaviour
    {
        public static VolumeManager Self { get; private set; }

        private const string MUSIC_VOLUME = "MusicVolume";
        private const string SFX_VOLUME = "SfxVolume";
        
        private const float START_VOLUME = 0.5f;

        [SerializeField] private AudioMixer _volumeAudioMixer;
        [field: SerializeField, ReadOnly] public float MusicSliderVolume { get; private set; }
        [field: SerializeField, ReadOnly] public float SfxSliderVolume { get; private set; }

        private void Awake()
        {
            if (Self != null && Self != this)
            {
                Destroy(gameObject);
            }
            else
            {
                DontDestroyOnLoad(gameObject);
                Self = this;
            }
        }

        private void Start()
        {
            SetVolume(AudioType.Music, START_VOLUME);
            SetVolume(AudioType.Sfx, START_VOLUME);
        }
        
        [UsedImplicitly]
        public void SetVolume(AudioType audioType, float val)
        {
            switch (audioType)
            {
                case AudioType.Sfx:
                {
                    MusicSliderVolume = val;
                    _volumeAudioMixer.SetFloat(MUSIC_VOLUME, Mathf.Log10(val) * 20);
                    
                    break;
                }

                case AudioType.Music:
                {
                    SfxSliderVolume = val;
                    _volumeAudioMixer.SetFloat(SFX_VOLUME, Mathf.Log10(val) * 20);
                    
                    break;
                }
            }
        }
    }
}