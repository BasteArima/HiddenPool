using UnityEngine;
using UniRx;

[CreateAssetMenu(menuName = "Data/MatchData")]
public class MatchData : ScriptableObject
{
    public enum State
    {
        None,
        MainMenu,
        Lobby,
        InitializeGame,
        Game,
        EndGame,
    }

    public enum MiniGames
    {
        None,
        HiddenPool
    }

    public ReactiveProperty<State> state = new ReactiveProperty<State>(State.None);
    public ReactiveProperty<MiniGames> game = new ReactiveProperty<MiniGames>(MiniGames.None);
}
