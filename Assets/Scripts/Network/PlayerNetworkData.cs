using System.Collections.Generic;
using System.Linq;
using Mirror;
using UnityEngine;

public class PlayerNetworkData : NetworkBehaviour
{
    [SerializeField] private AppData _data;
    
    [SerializeField, SyncVar] private string _playerName;
    
    public string PlayerName => _playerName;
    
    #region Client
    public override void OnStartClient()
    {
        _playerName = _data.userData.userName.Value;
    }
    
    public override void OnStopClient()
    {
        // disconnect event handlers
    }
    #endregion
}
