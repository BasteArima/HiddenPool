using System.Collections;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using Zenject;

public class PlayerProfileController : MonoBehaviour
{
    private const string PLAYER_AVATAR_SPRITE_FILE_NAME = "playerAvatar";
    
    [SerializeField] private TMP_InputField _nickNameInput;
    [SerializeField] private Button _playerAvatarChangeButton;
    [SerializeField] private Image _playerAvatarImage;

    private AppData _data;
    
    [Inject]
    private void Construct(AppData data)
    {
        _data = data;
    }

    private void Awake()
    {
        SetSubscribes();
    }

    private void SetSubscribes()
    {
        _nickNameInput.onValueChanged.AddListener(OnNicknameInputValueChanged);
        _playerAvatarChangeButton.onClick.AddListener(OnPlayerAvatarChangeButton);
    }

    private void Start()
    {
        _nickNameInput.text = _data.userData.userName.Value;
        LoadSavedSprite();
    }
    
    private void OnPlayerAvatarChangeButton()
    {
#if UNITY_STANDALONE_WIN
        OpenFileBrowser();
#else
        NativeGallery.GetImageFromGallery(LoadPNGFromDeviceGallery);
#endif
    }

    public void OpenFileBrowser()
    {
        // var bp = new BrowserProperties();
        // bp.filter = "Image files (*.jpg, *.jpeg, *.jpe, *.jfif, *.png) | *.jpg; *.jpeg; *.jpe; *.jfif; *.png";
        // bp.filterIndex = 0;
        //
        // new FileBrowser().OpenFileBrowser(bp, path =>
        // {
        //     //Load image from local path with UWR
        //     StartCoroutine(LoadImage(path));
        // });
    }

    IEnumerator LoadImage(string path)
    {
        using (UnityWebRequest uwr = UnityWebRequestTexture.GetTexture(path))
        {
            yield return uwr.SendWebRequest();

            if (uwr.isNetworkError || uwr.isHttpError)
            {
                Debug.Log(uwr.error);
            }
            else
            {
                var uwrTexture = DownloadHandlerTexture.GetContent(uwr);
                SaveAndLoadGotTexture(uwrTexture);
            }
        }
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

        SaveAndLoadGotTexture(tex);
    }

    private void SaveAndLoadGotTexture(Texture2D tex)
    {
        if (null == tex) return;
        
        var bytes = tex.EncodeToPNG();
        SaveSprite(bytes);
        var sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height),
            new Vector2(tex.width / 2, tex.height / 2));
        _playerAvatarImage.overrideSprite = sprite;

        _data.userData.userSprite = sprite;
        _data.userData.userAvatar = tex;
        _data.userData.userAvatarBytes = bytes;
    }

    private void SaveSprite(byte[] bytes)
    {
        var filePath = Application.dataPath + $"/Resources/{PLAYER_AVATAR_SPRITE_FILE_NAME}.png";
        File.WriteAllBytes(filePath, bytes);
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
            _data.userData.userSprite = loadedSprite;
            _data.userData.userAvatar = loadedSprite.texture;
            Debug.Log("Sprite loaded successfully!");

        }
        else
        {
            Debug.Log("Failed to load sprite.");
        }
    }
    
    private void OnNicknameInputValueChanged(string value)
    {
        _data.userData.userName.Value = value;
        SaveDataSystem.Instance.SaveData();
    }
}