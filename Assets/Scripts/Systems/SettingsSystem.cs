using System;
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
        SoundDesigner.GlobalMusicVolume = PlayerPrefs.GetFloat("MusicValue", 0.25f);

        SoundDesigner.SetVolumeAudioSource(SoundBaseType.Sound, SoundDesigner.GlobalSoundVolume);
        SoundDesigner.SetVolumeAudioSource(SoundBaseType.Music, SoundDesigner.GlobalMusicVolume);
    }

    private void SaveSettings()
    {

    }

    private void OnApplicationPause(bool pauseStatus)
    {
        SaveSettings();
    }

    private void OnApplicationQuit()
    {
        SaveSettings();
    }
}
