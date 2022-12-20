using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : BaseMenu
{
    [SerializeField] private Button _hostButton;
    [SerializeField] private Button _findGameButton;
    [SerializeField] private Button _aboutButton;
    [SerializeField] private Button _settingsButton;
    [SerializeField] private Button _exitButton;
    [SerializeField] private TMP_InputField _findGameCodeInput;
    [SerializeField] private TMP_InputField _nickNameInput;

    private void Awake()
    {
        _hostButton.onClick.AddListener(OnHostButton);
        _findGameButton.onClick.AddListener(OnFindGameButton);
        _aboutButton.onClick.AddListener(OnAboutButton);
        _settingsButton.onClick.AddListener(OnSettingsButton);
        _exitButton.onClick.AddListener(OnExitGameButton);
        _findGameCodeInput.onValueChanged.AddListener(OnFindGameCodeValueChanged);
        _nickNameInput.onValueChanged.AddListener(OnNicknameInputValueChanged);
        _findGameCodeInput.onValueChanged.Invoke("");
    }

    private void Start()
    {
        _nickNameInput.text = data.userData.userName.Value;
    }

    private void OnHostButton()
    {
        FirebaseController.Instance.CreateGame();
    }

    private void OnFindGameButton()
    {
        if (!FirebaseController.Instance.Initialized) return;
            FirebaseController.Instance.JoinGame(_findGameCodeInput.text.ToUpper());
    }

    private void OnAboutButton()
    {
        InterfaceManager.Toggle(MenuName.AboutMenu);
    }

    private void OnSettingsButton()
    {
        InterfaceManager.Toggle(MenuName.SettingsMenu);
    }

    private void OnExitGameButton()
    {
        Application.Quit();
    }

    private void OnFindGameCodeValueChanged(string value)
    {
        if (string.IsNullOrEmpty(value) || string.IsNullOrWhiteSpace(value) || value.Length < 4)
            _findGameButton.interactable = false;
        else
            _findGameButton.interactable = true;
    }
    
    private void OnNicknameInputValueChanged(string value)
    {
        data.userData.userName.Value = value;
        SaveDataSystem.Instance.SaveData();
    }
}