public class LobbyMenu : BaseMenu
{
    public void OnBackButton()
    {
        InterfaceManager.Toggle(MenuName.MainMenu);
    }
}
