using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RoomAvatarBanner : MonoBehaviour
{
    [SerializeField] private RectTransform _rectTransform;
    [SerializeField] private Image _avatarImage;
    [SerializeField] private TMP_Text _userName;

    public RectTransform Rect => _rectTransform;
    
    public void SetUserData(string name, byte[] avatar = null)
    {
        _userName.text = name;
        if(null != avatar)
            _avatarImage.overrideSprite = GetSpriteFromBytes(avatar);
    }

    private Sprite GetSpriteFromBytes(byte[] avatar)
    {
        var tex = new Texture2D(2, 2);
        tex.LoadImage(avatar);
        var sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(tex.width / 2, tex.height / 2));
        return sprite;
    }
}
