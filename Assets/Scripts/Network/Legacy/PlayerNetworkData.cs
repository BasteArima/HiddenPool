using UnityEngine;

public class PlayerNetworkData : MonoBehaviour
{
    [SerializeField] private AppData _data;
    
    [SerializeField] private string _playerName; // SyncVar
    [SerializeField] private Texture2D _playerAvatar;
    
    public string PlayerName => _playerName;
    public Texture2D PlayerAvatar => _playerAvatar;

    private void Awake()
    {
        _playerName = _data.userData.userName.Value;
        _playerAvatar = _data.userData.userAvatar;
    }
}
