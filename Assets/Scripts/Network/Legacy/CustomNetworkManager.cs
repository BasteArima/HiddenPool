using System;
using UnityEngine;
using Zenject;

public class CustomNetworkManager : MonoBehaviour
{
    public static CustomNetworkManager Instance;

    public static Action<PlayerNetworkData> ServerAddPlayer;

    private ChooseGameNetworkManager _chooseGameNetworkManager;
    
    [Inject]
    private void Construct(ChooseGameNetworkManager chooseGameNetworkManager)
    {
        _chooseGameNetworkManager = chooseGameNetworkManager;
    }

    public void Awake()
    {
        if (Instance != null) Destroy(Instance.gameObject);
        Instance = this;
    }

    public void OnServerAddPlayer()
    {
        
        // var newPlayer = conn.identity.GetComponent<PlayerNetworkData>();
        //
        // _chooseGameNetworkManager.Players.Add(newPlayer);
        // _chooseGameNetworkManager.OnClientGetInfoAddPlayer(newPlayer);
        //
        // ServerAddPlayer?.Invoke(newPlayer);
        // Debug.Log("New player connected: " + newPlayer.PlayerName);
    }
}
