using UnityEngine;

public class SettingsSystem : MonoBehaviour
{
    private void Awake()
    {
        Application.targetFrameRate = 60;
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        Time.timeScale = 1;
    }

    private void Start()
    {
        LoadSettings();
    }

    private void LoadSettings()
    {
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
