using System.Collections.Generic;
using UnityEngine;
using Steamworks;
using System.Linq;
using TMPro;

public class LobbyController : MonoBehaviour
{
    public static LobbyController Instance;

    // UI Elements
    public TMP_Text LobbyNameText;

    // Player Data
    public GameObject PlayerListViewContent;
    public GameObject PlayerListItemPrefab;
    public GameObject LocalPlayerObject;

    // Other Data
    public ulong CurrentLobbyId;
    public bool PlayerItemCreated = false;
    private List<PlayerListItem> PlayerListItems = new List<PlayerListItem>();
    public PlayerObjectController LocalPlayerController;

    // Manager
    private CustomNetworkManager manager;
    private CustomNetworkManager Manager
    {
        get
        {
            if (manager != null)
            {
                return manager;
            }
            return manager = CustomNetworkManager.singleton as CustomNetworkManager;
        }
    }

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    public void UpdateLobbyName()
    {
        CurrentLobbyId = Manager.GetComponent<SteamLobby>().CurrentLobbyID;
        LobbyNameText.text = SteamMatchmaking.GetLobbyData(new CSteamID(CurrentLobbyId), "name");
    }

    public void UpdatePlayerList()
    {
        if (!PlayerItemCreated) CreateHostPlayerItem(); // Host
        if (PlayerListItems.Count < manager.GamePlayers.Count) CreateClientPlayerItem();
        if (PlayerListItems.Count > manager.GamePlayers.Count) RemovePlayerItem();
        if (PlayerListItems.Count == manager.GamePlayers.Count) UpdatePlayerItem();
    }

    public void FindLocalPlayer()
    {
        LocalPlayerObject = GameObject.Find("LocalGamePlayer");
        LocalPlayerController = LocalPlayerObject.GetComponent<PlayerObjectController>();
    }

    public void CreateHostPlayerItem()
    {
        foreach (var player in Manager.GamePlayers)
        {
            var newPlayerItem = Instantiate(PlayerListItemPrefab) as GameObject;
            var newPlayerItemScript = newPlayerItem.GetComponent<PlayerListItem>();

            newPlayerItemScript.PlayerName = player.PlayerName;
            newPlayerItemScript.ConnectionId = player.ConnectionId;
            newPlayerItemScript.PlayerSteamId = player.PlayerSteamId;
            newPlayerItemScript.SetPlayerValues();

            newPlayerItem.transform.SetParent(PlayerListViewContent.transform);
            newPlayerItem.transform.localScale = Vector3.one;

            PlayerListItems.Add(newPlayerItemScript);
        }
        PlayerItemCreated = true;
    }

    public void CreateClientPlayerItem()
    {
        foreach (var player in Manager.GamePlayers)
        {
            if (!PlayerListItems.Any(b => b.ConnectionId == player.ConnectionId))
            {
                var newPlayerItem = Instantiate(PlayerListItemPrefab) as GameObject;
                var newPlayerItemScript = newPlayerItem.GetComponent<PlayerListItem>();

                newPlayerItemScript.PlayerName = player.PlayerName;
                newPlayerItemScript.ConnectionId = player.ConnectionId;
                newPlayerItemScript.PlayerSteamId = player.PlayerSteamId;
                newPlayerItemScript.SetPlayerValues();

                newPlayerItem.transform.SetParent(PlayerListViewContent.transform);
                newPlayerItem.transform.localScale = Vector3.one;

                PlayerListItems.Add(newPlayerItemScript);
            }
        }
    }

    public void UpdatePlayerItem()
    {
        foreach (var player in Manager.GamePlayers)
        {
            foreach(var playerListItemScript in PlayerListItems)
            {
                if(playerListItemScript.ConnectionId == player.ConnectionId)
                {
                    playerListItemScript.PlayerName = player.name;
                    playerListItemScript.SetPlayerValues();
                }
            }
        }
    }

    public void RemovePlayerItem()
    {
        List<PlayerListItem> playerListItemToRemove = new List<PlayerListItem>();

        foreach(var playerListeItem in PlayerListItems)
        {
            if(!Manager.GamePlayers.Any(b=>b.ConnectionId == playerListeItem.ConnectionId))
            {
                playerListItemToRemove.Add(playerListeItem);
            }
        }
        if(playerListItemToRemove.Count > 0)
        {
            foreach(var playerListItemToRemoveS in playerListItemToRemove)
            {
                var objectToRemove = playerListItemToRemoveS.gameObject;
                PlayerListItems.Remove(playerListItemToRemoveS);
                Destroy(objectToRemove);
                objectToRemove = null;
            }
        }
    }
}
