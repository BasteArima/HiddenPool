using Mirror;
using TMPro;
using UnityEngine;
using Zenject;

public class ChooseGameNetworkManager : NetworkBehaviour
{
    [SerializeField] private CardsGenerateSystem _cardsGenerateSystem;
    [SerializeField] private TMP_Text _roomCodeText;

    [Header("Game Room Settings")]
    [SerializeField, SyncVar] private string _roomCode;
    [SerializeField, SyncVar] private string _seed;
    [SerializeField, SyncVar] private string _cardsCount;

    [SerializeField] private SyncList<PlayerNetworkData> _players = new SyncList<PlayerNetworkData>();

    private AppData _data;
    private InterfaceManager _interfaceManager;
    
    public SyncList<PlayerNetworkData> Players => _players;

    [Inject]
    private void Construct(AppData data, InterfaceManager interfaceManager)
    {
        _data = data;
        _interfaceManager = interfaceManager;
    }
    
    public override void OnStartServer()
    {
        _roomCode = GetRandomRoomCode();
        _roomCodeText.text = _roomCode;
    }

    public override void OnStartClient()
    {
        _cardsGenerateSystem.Seed = _seed;
        _cardsGenerateSystem.CardsCount = _cardsCount;
        _roomCodeText.text = _roomCode;
        _cardsGenerateSystem.RestartGame();
    }

    public override void OnStopServer()
    {
        Debug.Log("OnStopServer");
    }

    public override void OnStopClient()
    {
        Debug.Log("OnStopClient");
        _data.matchData.state.Value = MatchData.State.EndGame;
        _interfaceManager.Toggle(MenuName.MainMenu);
    }

    private string GetRandomRoomCode()
    {
        var code = "";
        for (int i = 0; i < 4; i++)
            code += GetRandomLetter();
        return code;
    }

    private char GetRandomLetter()
    {
        var chars = "1234567890ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        var rand = new System.Random();
        var num = rand.Next(0, chars.Length);
        return chars[num];
    }
    
    public void ChangeGameOptions()
    {
        _seed = _cardsGenerateSystem.Seed;
        _cardsCount = _cardsGenerateSystem.CardsCount; 

        RestartGame();
    }

    private void RestartGame()
    {
        if(isServer)
            RpcRestart(_seed, _cardsCount);

        if (isClientOnly)
            CmdRestart(_seed, _cardsCount);
    }

    [Command(requiresAuthority = false)]
    private void CmdRestart(string seed, string cardsCount)
    {
        RpcRestart(seed, cardsCount);
    }

    [ClientRpc]
    private void RpcRestart(string seed, string cardsCount)
    {
        _cardsGenerateSystem.Seed = seed;
        _cardsGenerateSystem.CardsCount = cardsCount;
        _cardsGenerateSystem.RestartGame();
    }
    
    [ClientRpc]
    public void OnClientGetInfoAddPlayer(PlayerNetworkData newPlayer)
    {
        Debug.Log("New player connected: " + newPlayer.PlayerName);
    }

    public void OnHostGame()
    {
        _seed = _cardsGenerateSystem.Seed;
        _cardsCount = _cardsGenerateSystem.CardsCount;
    }
}