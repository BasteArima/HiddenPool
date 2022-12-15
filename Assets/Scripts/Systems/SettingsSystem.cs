using UnityEngine;

public class SettingsSystem : BaseMonoSystem
{
    private void Start()
    {
        LoadSettings();
    }

    private void LoadSettings()
    {
        SoundDesigner.GlobalSoundVolume = PlayerPrefs.GetFloat("SoundValue", 1);
        SoundDesigner.GlobalMusicVolume = PlayerPrefs.GetFloat("MusicValue", 1);

        SoundDesigner.SetVolumeAudioSource(SoundBaseType.Sound, SoundDesigner.GlobalSoundVolume);
        SoundDesigner.SetVolumeAudioSource(SoundBaseType.Music, SoundDesigner.GlobalMusicVolume);
    }

    public void SaveSettings()
    {
        PlayerPrefs.SetFloat("SoundValue", SoundDesigner.GlobalSoundVolume);
        PlayerPrefs.SetFloat("MusicValue", SoundDesigner.GlobalMusicVolume);
    }

}
