using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class MainMenu : BaseMenu
{
    [SerializeField] private Button _hostButton;
    [SerializeField] private Button _findGameButton;
    [SerializeField] private Button _aboutButton;
    [SerializeField] private Button _settingsButton;
    [SerializeField] private Button _exitButton;
    [SerializeField] private TMP_InputField _findGameCodeInput;

    private ChooseGameNetworkManager _chooseGameNetworkManager;
    
    [Inject]
    private void Construct(ChooseGameNetworkManager chooseGameNetworkManager)
    {
        _chooseGameNetworkManager = chooseGameNetworkManager;
    }
    
    private void Awake()
    {
        _hostButton.onClick.AddListener(OnHostButton);
        _findGameButton.onClick.AddListener(OnFindGameButton);
        _aboutButton.onClick.AddListener(OnAboutButton);
        _settingsButton.onClick.AddListener(OnSettingsButton);
        _exitButton.onClick.AddListener(OnExitGameButton);
        _findGameCodeInput.onValueChanged.AddListener(OnFindGameCodeValueChanged);
        _findGameCodeInput.onValueChanged.Invoke("");
    }

    private void OnHostButton()
    {
        CustomNetworkManager.Instance.StartHost();
        _data.matchData.state.Value = MatchData.State.InitializeGame;
        ChooseGameSystem.ChooseGame(MatchData.MiniGames.HiddenPool);
        _interfaceManager.Toggle(MenuName.HiddenPoolCoreMenu);
        _chooseGameNetworkManager.OnHostGame();
    }

    private void OnFindGameButton()
    {
        CustomNetworkManager.Instance.StartClient();
        _data.matchData.state.Value = MatchData.State.InitializeGame;
        ChooseGameSystem.ChooseGame(MatchData.MiniGames.HiddenPool);
        _interfaceManager.Toggle(MenuName.HiddenPoolCoreMenu);
    }

    private void OnAboutButton()
    {
        _interfaceManager.Toggle(MenuName.AboutMenu);
    }

    private void OnSettingsButton()
    {
        _interfaceManager.Toggle(MenuName.SettingsMenu);
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
}