using System.Linq;
using UniRx;
using UnityEngine;
using Zenject;

public class ChooseGameSystem : MonoBehaviour
{
    public static ChooseGameSystem Instance;

    private BaseGameSystem[] _gameSystems;

    private AppData _data;
    
    public MatchData matchData => _data.matchData;
    
    [Inject]
    private void Construct(AppData data)
    {
        _data = data;
        if (Instance != null) Destroy(Instance.gameObject);
        Instance = this;
        SetObservables();
    }
    
    private void SetObservables()
    {
        _data.matchData.state
            .Where(x => x == MatchData.State.MainMenu)
            .Subscribe(_ => FindGameSystems());

        _data.matchData.state
            .Where(x => x == MatchData.State.EndGame)
            .Subscribe(_ => EndGame());
    }

    private void FindGameSystems()
    {
        _gameSystems = FindObjectsOfType<BaseGameSystem>();
    }
    
    private static void EndGame()
    {
        Instance.matchData.game.Value = MatchData.MiniGames.None;
    }

    public static void ChooseGame(MatchData.MiniGames gameType)
    {
        Instance.matchData.game.Value = gameType;

        var game = Instance._gameSystems.SingleOrDefault(m => m.GameType == Instance.matchData.game.Value);
        if (game == null) return;
        game.Initialize();
        AdsManagerSystem.Instance.TryShowVideoAds();
    }

    public static void RestartGame()
    {
        var game = Instance._gameSystems.SingleOrDefault(m => m.GameType == Instance.matchData.game.Value);
        if (game == null) return;
        game.RestartGame();
    }
}
