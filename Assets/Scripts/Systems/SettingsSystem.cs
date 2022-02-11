using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingsSystem : BaseMonoSystem
{
    //[SerializeField] private Image _soundToggleIcon;
    //[SerializeField] private Image _musicToggleIcon;
    //[SerializeField] private TMP_Dropdown _langDropdown;

    //[Space]
    //[Tooltip("First sprite unmuted")]
    //[SerializeField] private Sprite[] _soundIcons;
    //[SerializeField] private Sprite[] _musicIcons;

    private void Start()
    {
        LoadSettings();
    }

    public void LoadSettings()
    {
        SoundDesigner.MusicMuted = Convert.ToBoolean(PlayerPrefs.GetInt("Music", 0));
        SoundDesigner.SoundMuted = Convert.ToBoolean(PlayerPrefs.GetInt("Sound", 0));

        //_soundToggleIcon.sprite = SoundDesigner.SoundMuted ? _soundIcons[0] : _soundIcons[1];
        //_musicToggleIcon.sprite = SoundDesigner.MusicMuted ? _musicIcons[0] : _musicIcons[1];

        SoundDesigner.SetMuteByBaseType(SoundBaseType.Sound, SoundDesigner.SoundMuted);
        SoundDesigner.SetMuteByBaseType(SoundBaseType.Music, SoundDesigner.MusicMuted);

        //_langDropdown.value = PlayerPrefs.GetInt("Language", 0);
        ChangeLanguage();
    }

    public void SaveSettings()
    {
        PlayerPrefs.SetInt("Music", Convert.ToInt32(SoundDesigner.MusicMuted));
        PlayerPrefs.SetInt("Sound", Convert.ToInt32(SoundDesigner.SoundMuted));

        PlayerPrefs.SetInt("Language", Convert.ToInt32(LocalizationSystem.GetCurrentLang()));
    }

    public void ToggleSound()
    {
        SoundDesigner.SoundMuted = !SoundDesigner.SoundMuted;
        SoundDesigner.SetMuteByBaseType(SoundBaseType.Sound, SoundDesigner.SoundMuted);
        //_soundToggleIcon.sprite = SoundDesigner.SoundMuted ? _soundIcons[0] : _soundIcons[1];
        SaveSettings();
    }

    public void ToggleMusic()
    {
        SoundDesigner.MusicMuted = !SoundDesigner.MusicMuted;
        SoundDesigner.SetMuteByBaseType(SoundBaseType.Music, SoundDesigner.MusicMuted);
        //_musicToggleIcon.sprite = SoundDesigner.MusicMuted ? _musicIcons[0] : _musicIcons[1];
        SaveSettings();
    }

    public void ChangeLanguage()
    {
        //Language language = (Language)Enum.Parse(typeof(Language), _langDropdown.value.ToString(), true);
        //LocalizationSystem.SetLanguage(language);
        //SaveSettings();
    }

    public void OnBackButton()
    {
        this.gameObject.SetActive(false);
    }
}
