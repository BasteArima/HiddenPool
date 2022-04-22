using Mirror;
using Steamworks;
using TMPro;
using UniRx;
using UnityEngine;

public class SteamLobby : BaseMonoSystem
{
    public static SteamLobby Instance;

    //Callbacks
    protected Callback<LobbyCreated_t> _lobbyCreated;
    protected Callback<GameLobbyJoinRequested_t> _joinRequest;
    protected Callback<LobbyEnter_t> _lobbyEntered;

    //Variables
    public ulong CurrentLobbyID;
    private const string _hostAddressKey = "HostAddress";
    private CustomNetworkManager _manager;

    //Gameobject
    [SerializeField] private TMP_Text _lobbyNameText;
    [SerializeField] private TMP_Text _everyoneText;
    [SerializeField] private TMP_Text _clienText;
    [SerializeField] private TMP_Text _everyoneTextTwo;
    [SerializeField] private TMP_Text _clienTextTwo;

    public override void Init(AppData data)
    {
        base.Init(data);
        SetObservables();
    }

    private void SetObservables()
    {
        data.matchData.state
            .Where(x => x == MatchData.State.Lobby)
            .Subscribe(_ => OnHostLobbyButton());
    }

    private void Start()
    {
        if (!SteamManager.Initialized) return;
        if (null == Instance) Instance = this;

        _manager = GetComponent<CustomNetworkManager>();

        _lobbyCreated = Callback<LobbyCreated_t>.Create(OnLobbyCreated);
        _joinRequest = Callback<GameLobbyJoinRequested_t>.Create(OnJoinRequest);
        _lobbyEntered = Callback<LobbyEnter_t>.Create(OnLobbyEntered);
    }

    private void OnHostLobbyButton()
    {
        SteamMatchmaking.CreateLobby(ELobbyType.k_ELobbyTypePublic, _manager.maxConnections);
    }

    public static void LeaveLobby()
    {
        SteamMatchmaking.LeaveLobby(new CSteamID(Instance.CurrentLobbyID));
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
        CurrentLobbyID = callback.m_ulSteamIDLobby;
        _lobbyNameText.gameObject.SetActive(true);
        _lobbyNameText.text = SteamMatchmaking.GetLobbyData(new CSteamID(callback.m_ulSteamIDLobby), "name");

        _everyoneText.text = "Everyone: " + Random.Range(0, 100).ToString();
        _clienText.text = "Client: " + Random.Range(0, 100).ToString();

        //Client
        if (NetworkServer.active) return;


        _everyoneTextTwo.text = "Everyone Two: " + Random.Range(0, 100).ToString();
        _clienTextTwo.text = "Client Two: " + Random.Range(0, 100).ToString();


        _manager.networkAddress = SteamMatchmaking.GetLobbyData(new CSteamID(callback.m_ulSteamIDLobby), _hostAddressKey);

        _manager.StartClient();
    }
}
