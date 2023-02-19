using System;
using Mirror;
using UnityEngine;

public class CustomNetworkManager : NetworkManager
{
    public static CustomNetworkManager Instance;

    public static Action<PlayerNetworkData> ServerAddPlayer;


    public override void Awake()
    {
        base.Awake();
        if (Instance != null) Destroy(Instance.gameObject);
        Instance = this;
    }

    public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    {
        base.OnServerAddPlayer(conn);
        
        var newPlayer = conn.identity.GetComponent<PlayerNetworkData>();
        
        ChooseGameNetworkManager.Instance.Players.Add(newPlayer);
        ChooseGameNetworkManager.Instance.OnClientGetInfoAddPlayer(newPlayer);
        
        ServerAddPlayer?.Invoke(newPlayer);
        Debug.Log("New player connected: " + newPlayer.PlayerName);
    }
}
