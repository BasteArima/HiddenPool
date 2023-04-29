using System;

public class LobbyMenu : BaseMenu
{
    protected override Action DoWhenPressEscape => OnBackButton;
    
    public void OnBackButton()
    {
        _interfaceManager.Toggle(MenuName.MainMenu);
    }
}
