using UnityEngine;
using UnityEngine.Audio;

namespace SkalluUtils.Utils
{
    public class VolumeManager : MonoBehaviour
    {
        [SerializeField] private AudioMixer _volumeAudioMixer;

        private void Start()
        {
            SetVolume("SFX", 0.5f);
            SetVolume("Music", 0.5f);
        }

        public void SetVolume(string groupName, float val)
        {
            _volumeAudioMixer.SetFloat(groupName, Mathf.Log10(val) * 20);
        }

        public float GetVolume(string groupName)
        {
            _volumeAudioMixer.GetFloat(groupName, out var volume);
            return volume;
        }
    }
}