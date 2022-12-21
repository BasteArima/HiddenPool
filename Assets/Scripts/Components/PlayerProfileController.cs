using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerProfileController : BaseMonoSystem
{
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
    }
    
    private void OnPlayerAvatarChangeButton()
    {
        NativeGallery.GetImageFromGallery(LoadPNG);
    }
    
    private void LoadPNG(string filePath) {
 
        Debug.Log($"Path: {filePath}");
        
        Texture2D tex = null;
        byte[] fileData;
 
        if (File.Exists(filePath))     {
            fileData = File.ReadAllBytes(filePath);
            tex = new Texture2D(2, 2);
            tex.LoadImage(fileData); //..this will auto-resize the texture dimensions.
        }
        
        //var bytes = tex.EncodeToPNG();
        var sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(tex.width / 2, tex.height / 2));
        _playerAvatarImage.overrideSprite = sprite;

        data.userData.userSprite = sprite;
        data.userData.userAvatar = tex;
    }

    private void OnNicknameInputValueChanged(string value)
    {
        data.userData.userName.Value = value;
        SaveDataSystem.Instance.SaveData();
    }
}