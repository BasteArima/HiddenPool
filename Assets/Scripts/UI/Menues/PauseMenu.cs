using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : BaseMenu
{
    [SerializeField] private Button _continueButton;
    [SerializeField] private Button _exitButton;

    private void Awake()
    {
        _continueButton.onClick.AddListener(OnContinueButton);
        _exitButton.onClick.AddListener(OnExitToMenuButton);
    }

    private void OnContinueButton()
    {
        InterfaceManager.TurnOnOff(MenuName.PauseMenu, false);
    }

    private void OnRestartButton()
    {
        ChooseGameSystem.RestartGame();
        InterfaceManager.TurnOnOff(MenuName.PauseMenu, false);
    }

    private void OnExitToMenuButton()
    {
        FirebaseController.Instance.RoomExit();
        data.matchData.state.Value = MatchData.State.EndGame;
        InterfaceManager.Toggle(MenuName.MainMenu);
    }
}