public class LobbyMenu : BaseMenu
{
    public void OnBackButton()
    {
        InterfaceManager.Toggle(MenuName.MainMenu);
    }

    public void OnClientButton()
    {
        InterfaceManager.Toggle(MenuName.ChoiseGameMenu);
    }
}
