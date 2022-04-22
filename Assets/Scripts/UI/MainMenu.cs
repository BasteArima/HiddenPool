using UnityEngine;

public class MainMenu : BaseMenu
{
    public void OnHostButton()
    {
        data.matchData.state.Value = MatchData.State.Lobby;
        InterfaceManager.Toggle(MenuName.LobbyMenu);
    }

    public void OnClientButton()
    {
        InterfaceManager.Toggle(MenuName.ChoiseGameMenu);
    }

    public void OnAboutButton()
    {
        InterfaceManager.Toggle(MenuName.AboutMenu);
    }

    public void OnSettingsButton()
    {
        InterfaceManager.Toggle(MenuName.SettingsMenu);
    }

    public void OnExitGameButton()
    {
        Application.Quit();
    }
}
