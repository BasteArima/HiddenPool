using System.Collections.Generic;
using UnityEngine;
using Mirror;
using Steamworks;

public class CustomNetworkManager : NetworkManager
{
    [SerializeField] private PlayerObjectController _gamePlayerPrefab;
    public List<PlayerObjectController> GamePlayers { get; } = new List<PlayerObjectController>();

    public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    {
        var gamePlayerInstance = Instantiate(_gamePlayerPrefab);
        gamePlayerInstance.ConnectionId = conn.connectionId;
        gamePlayerInstance.PlayerIdNumber = GamePlayers.Count + 1;
        gamePlayerInstance.PlayerSteamId = (ulong)SteamMatchmaking.GetLobbyMemberByIndex((CSteamID)SteamLobby.Instance.CurrentLobbyID, GamePlayers.Count);

        NetworkServer.AddPlayerForConnection(conn, gamePlayerInstance.gameObject);
    }
}
