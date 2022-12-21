using Firebase;
using Firebase.Database;
using Firebase.Extensions;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class FirebaseController : BaseMonoSystem
{
    public static FirebaseController Instance;

    public UnityEvent<string> PlayerOneJoined;
    public UnityEvent<string> PlayerTwoJoined;
    public UnityEvent PlayerOneExit;
    public UnityEvent PlayerTwoExit;

    [SerializeField] private CardsGenerateSystem _cardsGenerateSystem;
    [SerializeField] private TMP_Text _gameCodeText;
    [SerializeField] private string _dbLink = "https://hiddenpool-default-rtdb.europe-west1.firebasedatabase.app/";
    [SerializeField] private string _roomsParentName = "gameRooms";

    private FirebaseApp _app;
    private DatabaseReference _dbRef;
    private DatabaseReference _gameRoomRef;
    private string _roomCode;
    private bool _isPlayerOne;
    private bool _playerOneNull = true;
    private bool _playerTwoNull = true;

    public bool Initialized { get; private set; }

    #region Initialization

    public override void Init(AppData data)
    {
        base.Init(data);
        if (Instance != null) Destroy(Instance.gameObject);
        Instance = this;

        InitFirebase();
    }

    private void InitFirebase()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            var dependencyStatus = task.Result;
            if (dependencyStatus == DependencyStatus.Available)
            {
                _app = FirebaseApp.DefaultInstance;
                _dbRef = FirebaseDatabase.DefaultInstance.RootReference;
                _gameRoomRef = _dbRef.Child(_roomsParentName);
                Initialized = true;
            }
            else
            {
                Debug.LogError($"Could not resolve all Firebase dependencies: {dependencyStatus}");
                Initialized = false;
            }
        });
    }

    private void OnDestroy()
    {
        DeleteRoom();
    }

    private void OnApplicationQuit()
    {
        RoomExit();
        DeleteRoom();
    }

    #endregion

    #region CreateRoom

    private void CreateGame()
    {
        do
        {
            _roomCode = GetRandomRoomCode();
        } while (CheckCodeForDuplicate(_roomCode));

        _gameCodeText.text = _roomCode;
        CreateRoomOnDatabase(_cardsGenerateSystem.Seed, _cardsGenerateSystem.CardsCount);
    }

    private bool CheckCodeForDuplicate(string code)
    {
        bool isDuplicate = false;
        _gameRoomRef.GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError($"Failed get values from database");
            }
            else if (task.IsCompleted)
            {
                var snapshot = task.Result;
                if (snapshot.HasChild(code))
                    isDuplicate = true;
            }

            return isDuplicate;
        });

        return false;
    }

    private string GetRandomRoomCode()
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
        if (string.IsNullOrEmpty(_roomCode))
        {
            Debug.LogError("CreateRoomOnDatabase RoomCode is null or empty");
            return;
        }

        var room = new GameRoom(seed, cardsCount)
        {
            player1 = data.userData.userName.Value
        };
        
        PlayerOneJoined?.Invoke(room.player1);
        _isPlayerOne = true;
        _playerOneNull = false;
        
        var json = JsonUtility.ToJson(room);
        _gameRoomRef.Child(_roomCode).SetRawJsonValueAsync(json);
        _gameRoomRef.Child(_roomCode).Child("seed").ValueChanged += OnSeedValueChanged;
        _gameRoomRef.Child(_roomCode).Child("cardsCount").ValueChanged += OnCardsCountValueChanged;
        _gameRoomRef.Child(_roomCode).Child("player1").ValueChanged += OnPlayerOneValueChanged;
        _gameRoomRef.Child(_roomCode).Child("player2").ValueChanged += OnPlayerTwoValueChanged;
    }

    #endregion

    #region JoinRoom

    private void JoinGame(string code)
    {
        _roomCode = code;
        _gameCodeText.text = _roomCode;
        _gameRoomRef.Child(_roomCode).GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError($"Failed get values from database by code: {_roomCode}");
                return;
            }

            if (task.IsCompleted)
            {
                var snapshot = task.Result;
                var room = JsonUtility.FromJson<GameRoom>(snapshot.GetRawJsonValue());

                if (string.IsNullOrEmpty(room.player1))
                {
                    _gameRoomRef.Child(_roomCode).Child("player1").SetValueAsync(data.userData.userName.Value);
                    _isPlayerOne = true;
                    _playerOneNull = false;
                    if (string.IsNullOrEmpty(room.player2))
                    {
                        _playerTwoNull = true;
                    }
                    else
                    {
                        _playerTwoNull = false;
                        PlayerTwoJoined?.Invoke(room.player2);
                    }
                }
                else if (string.IsNullOrEmpty(room.player2))
                {
                    _gameRoomRef.Child(_roomCode).Child("player2").SetValueAsync(data.userData.userName.Value);
                    _isPlayerOne = false;
                    _playerTwoNull = false;
                    if (string.IsNullOrEmpty(room.player1))
                    {
                        _playerOneNull = true;
                    }
                    else
                    {
                        _playerOneNull = false;
                        PlayerOneJoined?.Invoke(room.player1);
                    }
                }

                _cardsGenerateSystem.Seed = room.seed;
                _cardsGenerateSystem.CardsCount = room.cardsCount;
            }
            else
            {
                Debug.LogError($"JoinGame error");
            }
        });

        _gameRoomRef.Child(_roomCode).Child("seed").ValueChanged += OnSeedValueChanged;
        _gameRoomRef.Child(_roomCode).Child("cardsCount").ValueChanged += OnCardsCountValueChanged;
        _gameRoomRef.Child(_roomCode).Child("player1").ValueChanged += OnPlayerOneValueChanged;
        _gameRoomRef.Child(_roomCode).Child("player2").ValueChanged += OnPlayerTwoValueChanged;

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

        if (string.IsNullOrEmpty(snapshot.Value.ToString())) // player one left
        {
            PlayerOneExit?.Invoke();
        }
        else // player one joined
        {
            PlayerOneJoined?.Invoke(snapshot.Value.ToString());
        }
        
        _playerOneNull = string.IsNullOrEmpty(snapshot.Value.ToString());
        if (_playerOneNull && _playerTwoNull)
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

        if (string.IsNullOrEmpty(snapshot.Value.ToString())) // player two left
        {
            PlayerTwoExit?.Invoke();
        }
        else // player two joined
        {
            PlayerTwoJoined?.Invoke(snapshot.Value.ToString());
        }

        _playerTwoNull = string.IsNullOrEmpty(snapshot.Value.ToString());
        if (_playerOneNull && _playerTwoNull)
            DeleteRoom();
    }

    public void RoomExit()
    {
        if (_isPlayerOne)
            _gameRoomRef.Child(_roomCode).Child("player1").SetValueAsync("");
        else
            _gameRoomRef.Child(_roomCode).Child("player2").SetValueAsync("");

        UnsubscribeAllRoomChilds();

        _isPlayerOne = false;
        _playerOneNull = true;
        _playerTwoNull = true;
    }

    public void UpdateRoomValuesOnDb(string seed, int cardsCount)
    {
        if (string.IsNullOrEmpty(_roomCode))
        {
            Debug.LogError("UpdateRoomValuesOnDb RoomCode is null or empty");
            return;
        }

        _gameRoomRef.Child(_roomCode).Child("seed").SetValueAsync(seed);
        _gameRoomRef.Child(_roomCode).Child("cardsCount").SetValueAsync(cardsCount);
    }

    private void UnsubscribeAllRoomChilds()
    {
        _gameRoomRef.Child(_roomCode).Child("seed").ValueChanged -= OnSeedValueChanged;
        _gameRoomRef.Child(_roomCode).Child("cardsCount").ValueChanged -= OnCardsCountValueChanged;
        _gameRoomRef.Child(_roomCode).Child("player1").ValueChanged -= OnPlayerOneValueChanged;
        _gameRoomRef.Child(_roomCode).Child("player2").ValueChanged -= OnPlayerTwoValueChanged;
    }

    private void DeleteRoom()
    {
        UnsubscribeAllRoomChilds();
        _gameRoomRef.Child(_roomCode).RemoveValueAsync();
    }

    #endregion

    #region PublicMethods

    public void CreateRoom()
    {
        CreateGame();
    }

    public void JoinRoom(string roomCode)
    {
        JoinGame(roomCode);
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