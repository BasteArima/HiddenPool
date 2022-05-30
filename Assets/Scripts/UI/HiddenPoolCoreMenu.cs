using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HiddenPoolCoreMenu : BaseMenu
{
    [SerializeField] private CardsGenerateSystem _cardsGenerateSystem;

    [Header("Buttons")]
    [SerializeField] private Button _refreshMainCardButton;
    [SerializeField] private Button _exitButton;
    [SerializeField] private Button _clearButton;
    [SerializeField] private Button _generateButton;
    [SerializeField] private Button _lockMainCardButton;

    [Header("Lock")]
    [SerializeField] private Image _lockMainCardImg;
    [SerializeField] private Sprite _lockMainCardLockedSprite;
    [SerializeField] private Sprite _lockMainCardUnlockedSprite;

    private void OnEnable()
    {
        _refreshMainCardButton.onClick.AddListener(OnRefreshMainCardButton);
        _exitButton.onClick.AddListener(OnExitButton);
        _clearButton.onClick.AddListener(OnClearButton);
        _generateButton.onClick.AddListener(OnGenerateButton);
        _lockMainCardButton.onClick.AddListener(OnLockButton);
    }

    public override void SetState(bool state)
    {
        base.SetState(state);
        if (state)
        {
            _lockMainCardImg.sprite = _cardsGenerateSystem.MainCardIsLocked ? _lockMainCardLockedSprite : _lockMainCardUnlockedSprite;
        }
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

    private void OnDisable()
    {
        _refreshMainCardButton.onClick.RemoveListener(OnRefreshMainCardButton);
        _exitButton.onClick.RemoveListener(OnExitButton);
        _clearButton.onClick.RemoveListener(OnClearButton);
        _generateButton.onClick.RemoveListener(OnGenerateButton);
        _lockMainCardButton.onClick.RemoveListener(OnLockButton);
    }
}
