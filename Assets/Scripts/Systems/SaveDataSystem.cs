using UnityEngine;
using Random = UnityEngine.Random;

public class SaveDataSystem : BaseMonoSystem
{
    public static SaveDataSystem Instance;
    
    [Header("Start values")]
    [SerializeField] private string _userName;
    [SerializeField] private int _sessionsCount;
    [SerializeField] private int _coins;
    [SerializeField] private int _amountWins;

    public override void Init(AppData data)
    {
        base.Init(data);

        if (Instance != null) Destroy(Instance.gameObject);
        Instance = this;
        
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
        PlayerPrefs.SetString("userName", data.userData.userName.Value);
        PlayerPrefs.SetInt("coins", data.userData.coins.Value);
        PlayerPrefs.SetInt("sessionsCount", data.userData.sessionsCount.Value);
        PlayerPrefs.SetInt("wins", data.userData.wins.Value);
    }

    private void LoadPlayerData()
    {
        data.userData.userName.Value = PlayerPrefs.GetString("userName", "Player_" + Random.Range(0,10000));
        data.userData.coins.Value = PlayerPrefs.GetInt("coins", _coins);
        data.userData.sessionsCount.Value = PlayerPrefs.GetInt("sessionsCount", _sessionsCount);
        data.userData.wins.Value = PlayerPrefs.GetInt("wins", _amountWins);
    }
}
