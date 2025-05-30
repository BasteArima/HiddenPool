using System;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : BaseMenu
{
    [SerializeField] private Button _continueButton;
    [SerializeField] private Button _exitButton;

    protected override Action DoWhenPressEscape => OnContinueButton;

    private void Awake()
    {
        _continueButton.onClick.AddListener(OnContinueButton);
        _exitButton.onClick.AddListener(OnExitToMenuButton);
    }

    private void OnContinueButton()
    {
        _interfaceManager.TurnOnOff(MenuName.PauseMenu, false);
    }

    private void OnExitToMenuButton()
    {
        _data.matchData.state.Value = MatchData.State.EndGame;
        _interfaceManager.Toggle(MenuName.MainMenu);
    }
}