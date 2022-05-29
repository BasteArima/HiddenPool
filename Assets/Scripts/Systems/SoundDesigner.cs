using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public class Sound
{
    public SoundType type;
    public SoundBaseType baseType;
    public AudioClip audioClip;
    public AudioSource audioSource;
    [Range(0f, 1f)]
    public float volume = 1;
    public float pitch = 1;
    public bool isLoop;
    public float pitchRange;
    public bool playOnAwake;
}

public enum SoundBaseType
{
    Sound, Music
}
public class SoundDesigner : BaseMonoSystem
{
    private static SoundDesigner _instance;
    public static float GlobalSoundVolume { get; set; }
    public static float GlobalMusicVolume { get; set; }
    public static bool SoundMuted { get; set; }
    public static bool MusicMuted { get; set; }

    public List<Sound> sounds;

    private void Awake()
    {
        if (_instance != null) Destroy(_instance.gameObject);
        _instance = this;

        foreach (Sound sound in sounds)
        {
            var newas = gameObject.AddComponent<AudioSource>();
            newas.clip = sound.audioClip;
            newas.loop = sound.isLoop;
            newas.playOnAwake = sound.playOnAwake;
            newas.pitch = sound.pitch;
            newas.volume = sound.volume;
            sound.audioSource = newas;
        }
    }

    private void Start()
    {
        SetMuteByBaseType(SoundBaseType.Sound, SoundMuted);
        SetMuteByBaseType(SoundBaseType.Music, MusicMuted);
    }

    /// <summary>
    /// Проигрывет звук данного типа
    /// </summary>
    /// <param name="type">Тип звука</param>
    public static void PlaySound(SoundType type)
    {
        var sound = _instance.sounds.SingleOrDefault(s => s.type == type);
        if (sound == null) return;
        sound.audioSource.pitch += Random.Range(-sound.pitchRange, sound.pitchRange);
        sound.audioSource.Play();
        sound.audioSource.pitch = sound.pitch;
    }

    /// <summary>
    /// Останавливает проигрывание звука данного типа
    /// </summary>
    /// <param name="type"></param>
    public static void StopSound(SoundType type)
    {
        var sound = _instance.sounds.SingleOrDefault(s => s.type == type);
        if (sound == null) return;
        sound.audioSource.Stop();
    }

    /// <summary>
    /// Мьютит заданный звук
    /// </summary>
    /// <param name="type"></param>
    public static void MuteSound(SoundType type, bool mute)
    {
        var sound = _instance.sounds.SingleOrDefault(s => s.type == type);
        if (sound == null) return;
        sound.audioSource.mute = mute;
    }

    /// <summary>
    /// Проигрывет звуки данного типа
    /// </summary>
    /// <param name="type">Базовый тип звука</param>
    public static void PlayMultipleSound(SoundType type)
    {
        foreach (var sound in _instance.sounds.Where(s => s.type == type))
            sound.audioSource.Play();
    }

    /// <summary>
    /// Влючает/выключает audio source привязанный к типу
    /// </summary>
    /// <param name="type">Тип звука к которому привязан audio source</param>
    /// <param name="state">Значение активности (enabled) </param>
    public static void SetActiveAudioSource(SoundType type, bool state)
    {
        var sound = _instance.sounds.SingleOrDefault(s => s.type == type);
        if (sound == null) return;
        sound.audioSource.enabled = state;
    }

    public static void SetVolumeAudioSource(SoundBaseType type, float volume)
    {
        foreach (var sound in _instance.sounds.Where(s => s.baseType == type))
        {
            sound.audioSource.volume = volume;
        }
    }

    public static void SetAllMuteAudioSource(bool state)
    {
        foreach (var sound in _instance.sounds)
            sound.audioSource.mute = state;
    }

    public static void SetMuteByBaseType(SoundBaseType type, bool state)
    {
        foreach (var sound in _instance.sounds.Where(s => s.baseType == type))
            sound.audioSource.mute = state;
    }
}
