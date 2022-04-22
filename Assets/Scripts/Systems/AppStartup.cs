using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SystemInitializer))]
public class AppStartup : MonoBehaviour
{
    [SerializeField] private AppData _data;

    private void Awake()
    {
        _data.matchData.state.Value = MatchData.State.AppStart;

        GetComponent<SystemInitializer>().InitializeSystems(_data);
    }
}
