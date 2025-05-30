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

    public ReactiveProperty<State> state = new ReactiveProperty<State>(State.None);
}
