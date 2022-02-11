using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemInitializer : MonoBehaviour
{
    [SerializeField] private AppData _data;
    [SerializeField] private BaseMonoSystem[] _systems;

    private void Awake()
    {
        _data.matchData.state.Value = MatchData.State.MainMenu;
        _data.matchData.game.Value = MatchData.MiniGames.None;

        foreach (var system in _systems)
        {
            system.Init(_data);
        }
    }
}
