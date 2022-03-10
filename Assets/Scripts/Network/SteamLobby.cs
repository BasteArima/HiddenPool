using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using Steamworks;
using TMPro;
using Sirenix.OdinInspector;
using UniRx;

public class SteamLobby : BaseMonoSystem
{
    //Callbacks
    protected Callback<LobbyCreated_t> _lobbyCreated;
    protected Callback<GameLobbyJoinRequested_t> _joinRequest;
    protected Callback<LobbyEnter_t> _lobbyEntered;

    //Variables
    [SerializeField, ReadOnly] private ulong _currentLobbyID;
    private const string _hostAddressKey = "HostAddress";
    private CustomNetworkManager _manager;

    //Gameobject
    [SerializeField] private TMP_Text _lobbyNameText;

    public override void Init(AppData data)
    {
        base.Init(data);
        SetObservables();
    }

    private void SetObservables()
    {
        data.matchData.state
            .Where(x => x == MatchData.State.InitializeGame)
            .Subscribe(_ => OnHostLobbyButton());
    }

    private void Start()
    {
        if (!SteamManager.Initialized) return;

        _manager = GetComponent<CustomNetworkManager>();

        _lobbyCreated = Callback<LobbyCreated_t>.Create(OnLobbyCreated);
        _joinRequest = Callback<GameLobbyJoinRequested_t>.Create(OnJoinRequest);
        _lobbyEntered = Callback<LobbyEnter_t>.Create(OnLobbyEntered);
    }

    private void OnHostLobbyButton()
    {
        SteamMatchmaking.CreateLobby(ELobbyType.k_ELobbyTypeFriendsOnly, _manager.maxConnections);
    }

    private void OnLobbyCreated(LobbyCreated_t callback)
    {
        if (callback.m_eResult != EResult.k_EResultOK) return;

        Debug.Log("Lobby created Succesfully");

        _manager.StartHost();

        SteamMatchmaking.SetLobbyData(new CSteamID(callback.m_ulSteamIDLobby), _hostAddressKey, SteamUser.GetSteamID().ToString());
        SteamMatchmaking.SetLobbyData(new CSteamID(callback.m_ulSteamIDLobby), "name", SteamFriends.GetPersonaName().ToString() + "'S LOBBY");
    }

    private void OnJoinRequest(GameLobbyJoinRequested_t callback)
    {
        Debug.Log("Request to joint lobby");
        SteamMatchmaking.JoinLobby(callback.m_steamIDLobby);
    }

    private void OnLobbyEntered(LobbyEnter_t callback)
    {
        //Everyone
        _currentLobbyID = callback.m_ulSteamIDLobby;
        _lobbyNameText.gameObject.SetActive(true);
        _lobbyNameText.text = SteamMatchmaking.GetLobbyData(new CSteamID(callback.m_ulSteamIDLobby), "name");

        //Client
        if (NetworkServer.active) return;

        _manager.networkAddress = SteamMatchmaking.GetLobbyData(new CSteamID(callback.m_ulSteamIDLobby), _hostAddressKey);

        _manager.StartClient();
    }
}
