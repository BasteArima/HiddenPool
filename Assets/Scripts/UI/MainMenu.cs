using Unity.Netcode;
using UnityEngine;

public class MainMenu : BaseMenu
{
    public void OnHostButton()
    {
        NetworkManager.Singleton.StartHost();
        InitializeCore();
    }

    public void OnClientButton()
    {
        //NetworkManager.Singleton.StartClient();
        //InitializeCore();

        InterfaceManager.Toggle(MenuName.ChoiseGameMenu);
    }

    public void OnServerButton()
    {
        NetworkManager.Singleton.StartServer();
        InitializeCore();
    }

    private void InitializeCore()
    {
        data.matchData.state.Value = MatchData.State.InitializeGame;
        InterfaceManager.Toggle(MenuName.HiddenPoolCoreMenu);
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
