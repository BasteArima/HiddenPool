using UnityEngine;

public class PauseMenu : BaseMenu
{
    public void OnSettingsButton()
    {
        //InterfaceManager.TurnOnOff(MenuName.PauseMenu, false);
        //InterfaceManager.TurnOnOff(MenuName.SettingsMenu, true);
    }

    public void OnContinueButton()
    {
        InterfaceManager.TurnOnOff(MenuName.PauseMenu, false);
    }

    public void OnRestartButton()
    {
        ChooseGameSystem.RestartGame();
        InterfaceManager.TurnOnOff(MenuName.PauseMenu, false);
    }

    public void OnGiveUpButton()
    {
        data.matchData.state.Value = MatchData.State.EndGame;
        InterfaceManager.Toggle(MenuName.MainMenu);
    }
}