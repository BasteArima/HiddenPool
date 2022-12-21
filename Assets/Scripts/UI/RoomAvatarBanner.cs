using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RoomAvatarBanner : MonoBehaviour
{
    [SerializeField] private RectTransform _rectTransform;
    [SerializeField] private Image _avatarImage;
    [SerializeField] private TMP_Text _userName;

    public RectTransform Rect => _rectTransform;
    
    public void SetUserData(string name, Sprite avatar = null)
    {
        _userName.text = name;
    }
}
