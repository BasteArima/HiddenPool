using Firebase;
using Firebase.Database;
using Firebase.Extensions;
using TMPro;
using UnityEngine;

public class FirebaseController : BaseMonoSystem
{
    public static FirebaseController Instance;

    [SerializeField] private CardsGenerateSystem _cardsGenerateSystem;
    [SerializeField] private TMP_Text _gameCodeText;

    private FirebaseApp _app;
    private DatabaseReference _dbRef;
    private string _dbLink = "https://hiddenpool-default-rtdb.europe-west1.firebasedatabase.app/";
    private bool _isPlayerOne;
    private bool _playerOneNull = true;
    private bool _playerTwoNull = true;

    public string RoomCode { get; private set; }

    public override void Init(AppData data)
    {
        base.Init(data);

        if (Instance != null) Destroy(Instance.gameObject);
        Instance = this;

        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            var dependencyStatus = task.Result;
            if (dependencyStatus == DependencyStatus.Available)
            {
                _app = FirebaseApp.DefaultInstance;
                _dbRef = FirebaseDatabase.DefaultInstance.RootReference.Child("gamerooms");
            }
            else
            {
                Debug.LogError(System.String.Format(
                    "Could not resolve all Firebase dependencies: {0}", dependencyStatus));
            }
        });
    }

    private void OnDestroy()
    {
        UnsubscribeAllRoomChilds();
    }

    private void OnApplicationQuit()
    {
        RoomExit();
        DeleteRoom();
    }

    #region CreateRoom

    public void CreateGame()
    {
        data.matchData.state.Value = MatchData.State.InitializeGame;
        ChooseGameSystem.ChooseGame(MatchData.MiniGames.HiddenPool);
        InterfaceManager.Toggle(MenuName.HiddenPoolCoreMenu);

        RoomCode = GetRandomRoomCode();
        _gameCodeText.text = RoomCode;
        CreateRoomOnDatabase(_cardsGenerateSystem.Seed, _cardsGenerateSystem.CardsCount);
    }

    private string GetRandomRoomCode() // [TODO] Check for existing code
    {
        string code = "";
        for (int i = 0; i < 4; i++)
            code += GetRandomLetter();
        return code;
    }

    private static char GetRandomLetter()
    {
        string chars = "1234567890ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        var rand = new System.Random();
        int num = rand.Next(0, chars.Length);
        return chars[num];
    }

    private void CreateRoomOnDatabase(string seed, string cardsCount)
    {
        if (string.IsNullOrEmpty(RoomCode))
        {
            Debug.LogError("CreateRoomOnDatabase RoomCode is null or empty");
            return;
        }

        var room = new GameRoom(seed, cardsCount);
        room.player1 = SystemInfo.deviceUniqueIdentifier;
        _isPlayerOne = true;
        _playerOneNull = false;
        var json = JsonUtility.ToJson(room);
        _dbRef.Child(RoomCode).SetRawJsonValueAsync(json);
        _dbRef.Child(RoomCode).Child("seed").ValueChanged += OnSeedValueChanged;
        _dbRef.Child(RoomCode).Child("cardsCount").ValueChanged += OnCardsCountValueChanged;
        _dbRef.Child(RoomCode).Child("player1").ValueChanged += OnPlayerOneValueChanged;
        _dbRef.Child(RoomCode).Child("player2").ValueChanged += OnPlayerTwoValueChanged;
    }

    #endregion

    #region JoinRoom

    public void JoinGame(string code)
    {
        RoomCode = code;
        _gameCodeText.text = RoomCode;
        _dbRef.Child(RoomCode).GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError($"Failed get values from database by code: {RoomCode}");
                return;
            }

            if (task.IsCompleted)
            {
                var snapshot = task.Result;
                var room = JsonUtility.FromJson<GameRoom>(snapshot.GetRawJsonValue());

                if (string.IsNullOrEmpty(room.player1))
                {
                    room.player1 = SystemInfo.deviceUniqueIdentifier;
                    _dbRef.Child(RoomCode).Child("player1").SetValueAsync(room.player1);
                    _isPlayerOne = true;
                    _playerOneNull = false;
                }
                else if (string.IsNullOrEmpty(room.player2))
                {
                    room.player2 = SystemInfo.deviceUniqueIdentifier;
                    _dbRef.Child(RoomCode).Child("player2").SetValueAsync(room.player2);
                    _isPlayerOne = false;
                    _playerTwoNull = false;
                }

                _cardsGenerateSystem.Seed = room.seed;
                _cardsGenerateSystem.CardsCount = room.cardsCount;
            }
            else
            {
                Debug.LogError($"JoinGame error");
            }
        });

        _dbRef.Child(RoomCode).Child("seed").ValueChanged += OnSeedValueChanged;
        _dbRef.Child(RoomCode).Child("cardsCount").ValueChanged += OnCardsCountValueChanged;
        _dbRef.Child(RoomCode).Child("player1").ValueChanged += OnPlayerOneValueChanged;
        _dbRef.Child(RoomCode).Child("player2").ValueChanged += OnPlayerTwoValueChanged;

        data.matchData.state.Value = MatchData.State.InitializeGame;
        ChooseGameSystem.ChooseGame(MatchData.MiniGames.HiddenPool);
        InterfaceManager.Toggle(MenuName.HiddenPoolCoreMenu);
    }

    #endregion

    #region InRoomMethods

    private void OnSeedValueChanged(object sender, ValueChangedEventArgs e)
    {
        if (e.DatabaseError != null)
        {
            Debug.LogError(e.DatabaseError.Message);
            return;
        }

        var snapshot = e.Snapshot;
        _cardsGenerateSystem.Seed = snapshot.Value.ToString();
        _cardsGenerateSystem.RestartGameByClient();
    }

    private void OnCardsCountValueChanged(object sender, ValueChangedEventArgs e)
    {
        if (e.DatabaseError != null)
        {
            Debug.LogError(e.DatabaseError.Message);
            return;
        }

        var snapshot = e.Snapshot;
        _cardsGenerateSystem.CardsCount = snapshot.Value.ToString();
        _cardsGenerateSystem.RestartGameByClient();
    }

    private void OnPlayerOneValueChanged(object sender, ValueChangedEventArgs e)
    {
        if (e.DatabaseError != null)
        {
            Debug.LogError(e.DatabaseError.Message);
            return;
        }

        var snapshot = e.Snapshot;
        _playerOneNull = string.IsNullOrEmpty(snapshot.Value.ToString());
        if(_playerOneNull && _playerTwoNull)
            DeleteRoom();
    }
    
    private void OnPlayerTwoValueChanged(object sender, ValueChangedEventArgs e)
    {
        if (e.DatabaseError != null)
        {
            Debug.LogError(e.DatabaseError.Message);
            return;
        }

        var snapshot = e.Snapshot;
        _playerTwoNull = string.IsNullOrEmpty(snapshot.Value.ToString());
        if(_playerOneNull && _playerTwoNull)
            DeleteRoom();
    }

    public void RoomExit()
    {
        _dbRef.Child(RoomCode).Child("seed").ValueChanged -= OnSeedValueChanged;
        _dbRef.Child(RoomCode).Child("cardsCount").ValueChanged -= OnCardsCountValueChanged;

        if (_isPlayerOne)
            _dbRef.Child(RoomCode).Child("player1").SetValueAsync("");
        else
            _dbRef.Child(RoomCode).Child("player2").SetValueAsync("");

        _dbRef.Child(RoomCode).Child("player1").ValueChanged -= OnPlayerOneValueChanged;
        _dbRef.Child(RoomCode).Child("player2").ValueChanged -= OnPlayerTwoValueChanged;
        
        _isPlayerOne = false;
        _playerOneNull = true;
        _playerTwoNull = true;
    }

    private void DeleteRoom()
    {
        UnsubscribeAllRoomChilds();
        _dbRef.Child(RoomCode).RemoveValueAsync();
    }

    private void UnsubscribeAllRoomChilds()
    {
        _dbRef.Child(RoomCode).Child("seed").ValueChanged -= OnSeedValueChanged;
        _dbRef.Child(RoomCode).Child("cardsCount").ValueChanged -= OnCardsCountValueChanged;
        _dbRef.Child(RoomCode).Child("player1").ValueChanged -= OnPlayerOneValueChanged;
        _dbRef.Child(RoomCode).Child("player2").ValueChanged -= OnPlayerTwoValueChanged;
    }

    public void UpdateRoomValuesOnDb(string seed, int cardsCount)
    {
        if (string.IsNullOrEmpty(RoomCode))
        {
            Debug.LogError("UpdateRoomValuesOnDb RoomCode is null or empty");
            return;
        }

        _dbRef.Child(RoomCode).Child("seed").SetValueAsync(seed);
        _dbRef.Child(RoomCode).Child("cardsCount").SetValueAsync(cardsCount);
    }

    #endregion
}

[System.Serializable]
public class GameRoom
{
    public string seed;
    public string cardsCount;
    public string player1;
    public string player2;

    public GameRoom()
    {
    }

    public GameRoom(string seed, string cardsCount)
    {
        this.seed = seed;
        this.cardsCount = cardsCount;
    }
}