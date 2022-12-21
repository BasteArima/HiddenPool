using DG.Tweening;
using UnityEngine;

public class RoomPlayerBannersController : BaseMonoSystem
{
    [SerializeField] private RoomAvatarBanner _playerOneAvatarBanner;
    [SerializeField] private RoomAvatarBanner _playerTwoAvatarBanner;

    [SerializeField] private float _hidedPositionY;
    [SerializeField] private float _openedPositionY;

    [SerializeField] private float _openDuration;
    [SerializeField] private float _hideDuration;

    public void OnPlayerOneJoined(string playerName)
    {
        Debug.Log($"OnPlayerOneJoined");
        _playerOneAvatarBanner.Rect.DOAnchorPosY(_openedPositionY, _openDuration);
        _playerOneAvatarBanner.SetUserData(playerName);
    }
    
    public void OnPlayerTwoJoined(string playerName)
    {
        Debug.Log($"OnPlayerTwoJoined");
        _playerTwoAvatarBanner.Rect.DOAnchorPosY(_openedPositionY, _openDuration);
        _playerTwoAvatarBanner.SetUserData(playerName);
    }
    
    public void OnPlayerOneExit()
    {
        Debug.Log($"OnPlayerOneExit");
        _playerOneAvatarBanner.Rect.DOAnchorPosY(_hidedPositionY, _hideDuration);
    }
    
    public void OnPlayerTwoExit()
    {
        Debug.Log($"OnPlayerTwoExit");
        _playerTwoAvatarBanner.Rect.DOAnchorPosY(_hidedPositionY, _hideDuration);
    }
}
