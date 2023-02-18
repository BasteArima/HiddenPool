using System;
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

    public override void Init(AppData data)
    {
        base.Init(data);
        SetSubscribes();
    }

    private void SetSubscribes()
    {
        FirebaseController.Instance.PlayerOneJoined += OnPlayerOneJoined;
        FirebaseController.Instance.PlayerTwoJoined += OnPlayerTwoJoined;
        FirebaseController.Instance.PlayerOneExit += OnPlayerOneExit;
        FirebaseController.Instance.PlayerTwoExit += OnPlayerTwoExit;
    }

    private void OnDestroy()
    {
        FirebaseController.Instance.PlayerOneJoined -= OnPlayerOneJoined;
        FirebaseController.Instance.PlayerTwoJoined -= OnPlayerTwoJoined;
        FirebaseController.Instance.PlayerOneExit -= OnPlayerOneExit;
        FirebaseController.Instance.PlayerTwoExit -= OnPlayerTwoExit;
    }

    private void OnPlayerOneJoined(string playerName, byte[] avatar = null)
    {
        Debug.Log($"OnPlayerOneJoined");
        _playerOneAvatarBanner.Rect.DOAnchorPosY(_openedPositionY, _openDuration);
        _playerOneAvatarBanner.SetUserData(playerName, avatar);
    }
    
    private void OnPlayerTwoJoined(string playerName, byte[] avatar = null)
    {
        Debug.Log($"OnPlayerTwoJoined");
        _playerTwoAvatarBanner.Rect.DOAnchorPosY(_openedPositionY, _openDuration);
        _playerTwoAvatarBanner.SetUserData(playerName, avatar);
    }
    
    private void OnPlayerOneExit()
    {
        Debug.Log($"OnPlayerOneExit");
        _playerOneAvatarBanner.Rect.DOAnchorPosY(_hidedPositionY, _hideDuration);
    }
    
    private void OnPlayerTwoExit()
    {
        Debug.Log($"OnPlayerTwoExit");
        _playerTwoAvatarBanner.Rect.DOAnchorPosY(_hidedPositionY, _hideDuration);
    }
}
