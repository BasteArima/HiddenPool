using Assets.SimpleLocalization;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HiddenPoolCoreMenu : BaseMenu
{
    [SerializeField] private CardsGenerateSystem _cardsGenerateSystem;

    [Header("Buttons")]
    [SerializeField] private Button _refreshMainCardButton;
    [SerializeField] private Button _exitButton;
    [SerializeField] private Button _pauseButton;
    [SerializeField] private Button _clearButton;
    [SerializeField] private Button _generateButton;
    [SerializeField] private Button _lockMainCardButton;
    [SerializeField] private Button _helpControlsButton;
    [SerializeField] private Button _modeButton;

    [Header("Lock")]
    [SerializeField] private Image _lockMainCardImg;
    [SerializeField] private Sprite _lockMainCardLockedSprite;
    [SerializeField] private Sprite _lockMainCardUnlockedSprite;

    [Header("Common")]
    [SerializeField] private GameObject _helpPanel;
    [SerializeField] private Image _modeImage;
    [SerializeField] private TMP_Text _modeText;
    [SerializeField] private Sprite _heroesModeSprite;
    [SerializeField] private Sprite _itemsModeSprite;
    [SerializeField] private Sprite _heroesItemsModeSprite;

    private int _modeBtnClickCount;

    private void Awake()
    {
        _refreshMainCardButton.onClick.AddListener(OnRefreshMainCardButton);
        _exitButton.onClick.AddListener(OnExitButton);
        _clearButton.onClick.AddListener(OnClearButton);
        _generateButton.onClick.AddListener(OnGenerateButton);
        _lockMainCardButton.onClick.AddListener(OnLockButton);
        _helpControlsButton.onClick.AddListener(OnHelpControlsButton);
        _modeButton.onClick.AddListener(OnGenerateModeButton);
        _pauseButton.onClick.AddListener(OnPauseButton);
    }

    public override void SetState(bool state)
    {
        base.SetState(state);
        if (state)
        {
            _lockMainCardImg.sprite = _cardsGenerateSystem.MainCardIsLocked ? _lockMainCardLockedSprite : _lockMainCardUnlockedSprite;
        }
    }

    private void OnGenerateModeButton()
    {
        _modeBtnClickCount++;

        if (_modeBtnClickCount > System.Enum.GetNames(typeof(CardsGenerateSystem.CardGenerateModes)).Length - 1)
            _modeBtnClickCount = 0;

        if (_modeBtnClickCount == 0)
        {
            _modeImage.sprite = _heroesModeSprite;
            _modeText.text = LocalizationManager.Localize("HiddenPoolCore.ModeHeroes");
        }
        else if(_modeBtnClickCount == 1)
        {
            _modeImage.sprite = _itemsModeSprite;
            _modeText.text =  LocalizationManager.Localize("HiddenPoolCore.ModeItems");
        }
        else if (_modeBtnClickCount == 2)
        {
            _modeImage.sprite = _heroesItemsModeSprite;
            _modeText.text = LocalizationManager.Localize("HiddenPoolCore.ModeHeroItems");
        }

        _cardsGenerateSystem.SetGenerateMode((CardsGenerateSystem.CardGenerateModes)_modeBtnClickCount);
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
        _cardsGenerateSystem.RestartGame();
    }

    private void OnClearButton()
    {
        _cardsGenerateSystem.RefreshCards();
    }

    private void OnExitButton()
    {
        FirebaseController.Instance.RoomExit();
        _modeBtnClickCount = 0;
        data.matchData.state.Value = MatchData.State.EndGame;
        InterfaceManager.Toggle(MenuName.MainMenu);
    }

    private void OnRefreshMainCardButton()
    {
        _cardsGenerateSystem.ChoiseRandomCard();
    }

    private void OnLockButton()
    {
        _cardsGenerateSystem.ToggleLockMainCard();
        _lockMainCardImg.sprite = _cardsGenerateSystem.MainCardIsLocked ? _lockMainCardLockedSprite : _lockMainCardUnlockedSprite;
    }
}
