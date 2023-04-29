using Mirror;
using UnityEngine;

public class PlayerNetworkData : NetworkBehaviour
{
    [SerializeField] private AppData _data;
    
    [SerializeField, SyncVar] private string _playerName;
    [SerializeField, SyncVar] private Texture2D _playerAvatar;
    
    public string PlayerName => _playerName;
    public Texture2D PlayerAvatar => _playerAvatar;

    private void Awake()
    {
        _playerName = _data.userData.userName.Value;
        _playerAvatar = _data.userData.userAvatar;
    }
}
