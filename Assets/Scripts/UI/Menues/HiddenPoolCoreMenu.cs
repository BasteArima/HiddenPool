using System;
using UnityEngine;
using UnityEngine.UI;

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

    [Header("Lock")]
    [SerializeField] private Image _lockMainCardImg;
    [SerializeField] private Sprite _lockMainCardLockedSprite;
    [SerializeField] private Sprite _lockMainCardUnlockedSprite;

    [Header("Common")]
    [SerializeField] private GameObject _helpPanel;
    
    protected override Action DoWhenPressEscape => OnPauseButton;

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
        _interfaceManager.TurnOnOff(MenuName.PauseMenu, true);
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
        _data.matchData.state.Value = MatchData.State.EndGame;
        _interfaceManager.Toggle(MenuName.MainMenu);
    }

    private void OnRefreshMainCardButton()
    {
        if(!_cardsGenerateSystem.MainCardIsLocked)
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