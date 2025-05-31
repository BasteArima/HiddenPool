using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SettingsMenu : BaseMenu
{
    [SerializeField] private Button _backButton;
    [SerializeField] private Button _toggleScreenModeButton;
    [SerializeField] private Slider _soundSlider;
    [SerializeField] private Slider _musicSlider;
    [SerializeField] private TMP_Text _soundValueText;
    [SerializeField] private TMP_Text _musicValueText;
    
    protected override Action DoWhenPressEscape => OnBackButton;
    
    public override void SetState(bool state)
    {
        base.SetState(state);
        if (state)
        {
            _soundSlider.value = SoundDesigner.GlobalSoundVolume;
            _musicSlider.value = SoundDesigner.GlobalMusicVolume;

            _soundValueText.text = (_soundSlider.value * 100).ToString("F0");
            _musicValueText.text = (_musicSlider.value * 100).ToString("F0");
        }
    }

    private void Awake()
    {
        _backButton.onClick.AddListener(OnBackButton);
        _toggleScreenModeButton.onClick.AddListener(OnToggleScreenModeButton);
        _soundSlider.onValueChanged.AddListener(OnSoundValueChanges);
        _musicSlider.onValueChanged.AddListener(OnMusicValueChanges);
    }

    private void OnSoundValueChanges(float value)
    {
        SoundDesigner.SetVolumeAudioSource(SoundBaseType.Sound, value);
        SoundDesigner.GlobalSoundVolume = value;
        _soundValueText.text = (value * 100).ToString("F0");
        PlayerPrefs.SetFloat("SoundValue", value);
    }

    private void OnMusicValueChanges(float value)
    {
        SoundDesigner.SetVolumeAudioSource(SoundBaseType.Music, value);
        SoundDesigner.GlobalMusicVolume = value;
        _musicValueText.text = (value * 100).ToString("F0");
        PlayerPrefs.SetFloat("MusicValue", value);
    }

    private void OnToggleScreenModeButton()
    {
        if (!Screen.fullScreen) 
            Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
        else 
            Screen.fullScreenMode = FullScreenMode.Windowed;
    }

    private void OnBackButton()
    {
        _interfaceManager.Toggle(MenuName.MainMenu);
    }
}
