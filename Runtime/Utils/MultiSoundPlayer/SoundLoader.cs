namespace SkalluUtils.Utils.MultiSoundPlayer
{
    public static class SoundLoader
    {
        public static void LoadSounds(MultiSoundPlayer multiSoundPlayer, System.Collections.Generic.List<SoundClip> sounds)
        {
            if (sounds == null || sounds.Count == 0)
            {
                UnityEngine.Debug.LogError($"Cannot load sounds, because sound list is null or empty!");
                return;
            }

            if (multiSoundPlayer == null)
            {
                UnityEngine.Debug.LogError($"Cannot load sounds, because MultiSoundPlayer is null!");
                return;
            }
            
            multiSoundPlayer.LoadSounds(sounds);
        }
    }
}