using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class HiddenPoolCoreMenu : BaseMenu
{
    [SerializeField] private CardsGenerateSystem _cardsGenerateSystem;
    [SerializeField] private ChooseGameNetworkManager _chooseGameNetworkManager;

    [Header("Buttons")]
    [SerializeField] private Button _refreshMainCardButton;
    [SerializeField] private Button _exitButton;
    [SerializeField] private Button _pauseButton;
    [SerializeField] private Button _clearButton;
    [SerializeField] private Button _generateButton;
    [SerializeField] private Button _lockMainCardButton;
    [SerializeField] private Button _helpControlsButton;

    [Header("Lock")]
    [SerializeField] private Image _lockMainCardImg;
    [SerializeField] private Sprite _lockMainCardLockedSprite;
    [SerializeField] private Sprite _lockMainCardUnlockedSprite;

    [Header("Common")]
    [SerializeField] private GameObject _helpPanel;

    private void Awake()
    {
        _refreshMainCardButton.onClick.AddListener(OnRefreshMainCardButton);
        _exitButton.onClick.AddListener(OnExitButton);
        _clearButton.onClick.AddListener(OnClearButton);
        _generateButton.onClick.AddListener(OnGenerateButton);
        _lockMainCardButton.onClick.AddListener(OnLockButton);
        _helpControlsButton.onClick.AddListener(OnHelpControlsButton);
        _pauseButton.onClick.AddListener(OnPauseButton);
    }

    public override void SetState(bool state)
    {
        base.SetState(state);
        if (state)
        {
            _lockMainCardImg.sprite = _cardsGenerateSystem.MainCardIsLocked
                ? _lockMainCardLockedSprite
                : _lockMainCardUnlockedSprite;
        }
    }

    private void OnPauseButton()
    {
        InterfaceManager.TurnOnOff(MenuName.PauseMenu, true);
    }

    private void OnHelpControlsButton()
    {
        _helpPanel.SetActive(!_helpPanel.activeSelf);
    }

    private void OnGenerateButton()
    {
        _chooseGameNetworkManager.ChangeGameOptions();

        //_cardsGenerateSystem.RestartGame();
    }

    private void OnClearButton()
    {
        _cardsGenerateSystem.RefreshCards();
    }

    private void OnExitButton()
    {
        //FirebaseController.Instance.RoomExit();

        if (NetworkServer.active && NetworkClient.isConnected)
            NetworkManager.singleton.StopHost();
        else if (NetworkClient.isConnected)
            NetworkManager.singleton.StopClient();
        else if (NetworkServer.active)
            NetworkManager.singleton.StopServer();

        data.matchData.state.Value = MatchData.State.EndGame;
        InterfaceManager.Toggle(MenuName.MainMenu);
    }

    private void OnRefreshMainCardButton()
    {
        _cardsGenerateSystem.ChoiceRandomCard();
    }

    private void OnLockButton()
    {
        _cardsGenerateSystem.ToggleLockMainCard();
        _lockMainCardImg.sprite = _cardsGenerateSystem.MainCardIsLocked
            ? _lockMainCardLockedSprite
            : _lockMainCardUnlockedSprite;
    }
}