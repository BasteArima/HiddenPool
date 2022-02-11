using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;

public class SaveDataSystem : BaseMonoSystem
{
    [Header("Start values")]
    [SerializeField] private string _userName;
    [SerializeField] private int _sessionsCount;
    [SerializeField] private int _coins;
    [SerializeField] private int _amountWins;

    private void Start()
    {
        LoadPlayerData();
    }

    private void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus)
            SavePlayerData();
    }

    private void OnApplicationQuit() => SavePlayerData();

    public void SaveData() => SavePlayerData();

    private void SavePlayerData()
    {
        PlayerPrefs.SetInt("coins", data.userData.coins.Value);
        PlayerPrefs.SetInt("sessionsCount", data.userData.sessionsCount.Value);
        PlayerPrefs.SetInt("wins", data.userData.wins.Value);
    }

    private void LoadPlayerData()
    {
        data.userData.coins.Value = PlayerPrefs.GetInt("coins", _coins);
        data.userData.sessionsCount.Value = PlayerPrefs.GetInt("sessionsCount", _sessionsCount);
        data.userData.wins.Value = PlayerPrefs.GetInt("wins", _amountWins);
    }
}
