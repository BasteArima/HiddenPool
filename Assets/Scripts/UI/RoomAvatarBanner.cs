using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RoomAvatarBanner : MonoBehaviour
{
    [SerializeField] private RectTransform _rectTransform;
    [SerializeField] private Image _avatarImage;
    [SerializeField] private TMP_Text _userName;

    public RectTransform Rect => _rectTransform;
    public bool isOpened { get; set; }
    
    public void SetUserData(string name, Texture2D avatar = null)
    {
        _userName.text = name;
        if(null != avatar)
            _avatarImage.overrideSprite = GetSpriteFromBytes(avatar);
    }

    private Sprite GetSpriteFromBytes(Texture2D avatar)
    {
        var sprite = Sprite.Create(avatar, new Rect(0, 0, avatar.width, avatar.height), new Vector2(avatar.width / 2, avatar.height / 2));
        return sprite;
    }
}
