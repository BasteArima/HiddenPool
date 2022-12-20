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
        
        _nickNameInput.onValueChanged.AddListener(OnNicknameInputValueChanged);
        _playerAvatarChangeButton.onClick.AddListener(OnPlayerAvatarChangeButton);
    }
    
    private void Start()
    {
        _nickNameInput.text = data.userData.userName.Value;
    }

    private void OnPlayerAvatarChangeButton()
    {
        
    }
    
    private void OnNicknameInputValueChanged(string value)
    {
        data.userData.userName.Value = value;
        SaveDataSystem.Instance.SaveData();
    }
}
