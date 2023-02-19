using System.IO;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class PlayerProfileController : BaseMonoSystem
{
    private const string PLAYER_AVATAR_SPRITE_FILE_NAME = "playerAvatar";
    
    [SerializeField] private TMP_InputField _nickNameInput;
    [SerializeField] private Button _playerAvatarChangeButton;
    [SerializeField] private Image _playerAvatarImage;

    public override void Init(AppData data)
    {
        base.Init(data);
        SetSubscribes();
    }

    private void SetSubscribes()
    {
        _nickNameInput.onValueChanged.AddListener(OnNicknameInputValueChanged);
        _playerAvatarChangeButton.onClick.AddListener(OnPlayerAvatarChangeButton);
    }

    private void Start()
    {
        _nickNameInput.text = data.userData.userName.Value;
        LoadSavedSprite();
    }
    
    private void OnPlayerAvatarChangeButton()
    {
        NativeGallery.GetImageFromGallery(LoadPNGFromDeviceGallery);
    }
    
    private void LoadPNGFromDeviceGallery(string filePath) {
 
        Debug.Log($"Path: {filePath}");

        Texture2D tex = null;
        byte[] fileData;
 
        if (File.Exists(filePath))     {
            fileData = File.ReadAllBytes(filePath);
            tex = new Texture2D(2, 2);
            tex.LoadImage(fileData); // this will auto-resize the texture dimensions.
        }

        if (null != tex)
        {
            var bytes = tex.EncodeToPNG();
            SaveSprite(bytes);
            var sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height),
                new Vector2(tex.width / 2, tex.height / 2));
            _playerAvatarImage.overrideSprite = sprite;

            data.userData.userSprite = sprite;
            data.userData.userAvatar = tex;
            data.userData.userAvatarBytes = bytes;
        }
    }

    private void SaveSprite(byte[] bytes)
    {
        var filePath = Application.dataPath + $"/Resources/{PLAYER_AVATAR_SPRITE_FILE_NAME}.png";
        File.WriteAllBytes(filePath, bytes);
        AssetDatabase.Refresh();
        if (Resources.Load<Sprite>(PLAYER_AVATAR_SPRITE_FILE_NAME) != null)
            Debug.Log("Sprite saved successfully!");
        else
            Debug.Log("Failed to save sprite.");
    }

    private void LoadSavedSprite()
    {
        var loadedSprite = Resources.Load<Sprite>(PLAYER_AVATAR_SPRITE_FILE_NAME);

        if (loadedSprite != null)
        {
            _playerAvatarImage.overrideSprite = loadedSprite;
            data.userData.userSprite = loadedSprite;
            data.userData.userAvatar = loadedSprite.texture;
            Debug.Log("Sprite loaded successfully!");

        }
        else
        {
            Debug.Log("Failed to load sprite.");
        }
    }
    
    private void OnNicknameInputValueChanged(string value)
    {
        data.userData.userName.Value = value;
        SaveDataSystem.Instance.SaveData();
    }
}