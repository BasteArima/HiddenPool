using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseGameSystem : MonoBehaviour
{
    [SerializeField] protected MatchData.MiniGames _gameType;
    public MatchData.MiniGames GameType => _gameType;

    public virtual void Initialize() { }
    public virtual void RestartGame() { }
    public virtual void EndGame() { }
}
