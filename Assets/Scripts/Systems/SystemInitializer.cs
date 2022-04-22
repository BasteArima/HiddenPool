using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemInitializer : MonoBehaviour
{
    [SerializeField] private BaseMonoSystem[] _systems;

    public void InitializeSystems(AppData data)
    {
        data.matchData.state.Value = MatchData.State.MainMenu;
        data.matchData.game.Value = MatchData.MiniGames.None;

        foreach (var system in _systems)
        {
            system.Init(data);
        }
    }
}
