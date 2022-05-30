using System;
using UnityEngine;

public class SettingsSystem : BaseMonoSystem
{
    private void Start()
    {
        LoadSettings();
    }

    public void LoadSettings()
    {
        SoundDesigner.GlobalSoundVolume = PlayerPrefs.GetFloat("SoundValue", 1);
        SoundDesigner.GlobalMusicVolume = PlayerPrefs.GetFloat("MusicValue", 1);

        SoundDesigner.SetVolumeAudioSource(SoundBaseType.Sound, SoundDesigner.GlobalSoundVolume);
        SoundDesigner.SetVolumeAudioSource(SoundBaseType.Music, SoundDesigner.GlobalMusicVolume);

        //_langDropdown.value = PlayerPrefs.GetInt("Language", 0);
        //ChangeLanguage();
    }

    public void SaveSettings()
    {
        PlayerPrefs.SetFloat("SoundValue", SoundDesigner.GlobalSoundVolume);
        PlayerPrefs.SetFloat("MusicValue", SoundDesigner.GlobalMusicVolume);

        PlayerPrefs.SetInt("Language", Convert.ToInt32(LocalizationSystem.GetCurrentLang()));
    }

    public void ChangeLanguage()
    {
        //Language language = (Language)Enum.Parse(typeof(Language), _langDropdown.value.ToString(), true);
        //LocalizationSystem.SetLanguage(language);
        //SaveSettings();
    }
}
