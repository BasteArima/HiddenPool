using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

public class SaveDataSystem : MonoBehaviour
{
    public static SaveDataSystem Instance;
    
    [Header("Start values")]
    [SerializeField] private string _userName;
    [SerializeField] private int _sessionsCount;
    [SerializeField] private int _coins;
    [SerializeField] private int _amountWins;

    private AppData _data;
    
    [Inject]
    private void Construct(AppData data)
    {
        _data = data;
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
        PlayerPrefs.SetString("userName", _data.userData.userName.Value);
        PlayerPrefs.SetInt("coins", _data.userData.coins.Value);
        PlayerPrefs.SetInt("sessionsCount", _data.userData.sessionsCount.Value);
        PlayerPrefs.SetInt("wins", _data.userData.wins.Value);
    }

    private void LoadPlayerData()
    {
        _data.userData.userName.Value = PlayerPrefs.GetString("userName", "Player_" + Random.Range(0,10000));
        _data.userData.coins.Value = PlayerPrefs.GetInt("coins", _coins);
        _data.userData.sessionsCount.Value = PlayerPrefs.GetInt("sessionsCount", _sessionsCount);
        _data.userData.wins.Value = PlayerPrefs.GetInt("wins", _amountWins);
    }
}
