using System;
using UnityEngine;

namespace SkalluUtils.Utils.Sound
{
    [Serializable]
    public class Sound
    {
        [SerializeField] private AudioClip clip;
        public AudioClip Clip => clip;
        
        [SerializeField] [Range(0, 1)] private float volume;
        public float Volume => volume;
        
        [SerializeField] [Range(0, 3)] private float pitch;
        public float Pitch => pitch;
    }
}
