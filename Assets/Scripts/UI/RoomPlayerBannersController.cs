using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class RoomPlayerBannersController : BaseMonoSystem
{
    [SerializeField] private List<RoomAvatarBanner> _avatarsBanners;
    
    [SerializeField] private float _hidedPositionY;
    [SerializeField] private float _openedPositionY;

    [SerializeField] private float _openDuration;
    [SerializeField] private float _hideDuration;

    private void Start()
    {
        SetSubscribes();
    }

    private void SetSubscribes()
    {
        CustomNetworkManager.ServerAddPlayer += OnServerAddPlayer;
    }

    private void OnDestroy()
    {
        CustomNetworkManager.ServerAddPlayer -= OnServerAddPlayer;
    }

    private void OnServerAddPlayer(PlayerNetworkData player)
    {
        foreach (var banner in _avatarsBanners)
        {
            if (!banner.isOpened)
            {
                banner.Rect.DOAnchorPosY(_openedPositionY, _openDuration);
                banner.SetUserData(player.PlayerName, null);
                break;
            }
        }
    }
}
