using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : BaseMenu
{
    [SerializeField] private CardsGenerateSystem _cardsGenerateSystem;
    [SerializeField] private Button _hostButton;
    [SerializeField] private Button _findGameButton;
    [SerializeField] private Button _aboutButton;
    [SerializeField] private Button _settingsButton;
    [SerializeField] private Button _exitButton;
    [SerializeField] private TMP_InputField _findGameCodeInput;
    [SerializeField] private ConfirmWindowView _exitWarning;
    
    private void Awake()
    {
        _hostButton.onClick.AddListener(OnCreateGame);
        _findGameButton.onClick.AddListener(OnFindGameButton);
        _aboutButton.onClick.AddListener(OnAboutButton);
        _settingsButton.onClick.AddListener(OnSettingsButton);
        _exitButton.onClick.AddListener(OnExitGameButton);
        _findGameCodeInput.onValueChanged.AddListener(OnFindGameCodeValueChanged);
        _findGameCodeInput.onValueChanged.Invoke("");
    }

    private void OnCreateGame()
    {
        _data.matchData.state.Value = MatchData.State.InitializeGame;
        _cardsGenerateSystem.Initialize();
        //AdsManagerSystem.Instance.TryShowVideoAds();
        _interfaceManager.Toggle(MenuName.HiddenPoolCoreMenu);
    }

    private void OnFindGameButton()
    {
        OnCreateGame();
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
        _exitWarning.gameObject.SetActive(true);
        _exitWarning.ConfirmAction(Application.Quit);
    }

    private void OnFindGameCodeValueChanged(string value)
    {
        if (string.IsNullOrEmpty(value) || string.IsNullOrWhiteSpace(value) || value.Length < 4)
            _findGameButton.interactable = false;
        else
            _findGameButton.interactable = true;
    }
}