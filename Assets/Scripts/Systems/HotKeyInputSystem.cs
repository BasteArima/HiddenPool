using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HotKeyInputSystem : BaseMonoSystem
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!InterfaceManager.GetMenuActive(MenuName.MainMenu) 
                && !InterfaceManager.GetMenuActive(MenuName.ChoiseGameMenu)
                && !InterfaceManager.GetMenuActive(MenuName.AboutMenu)
                && !InterfaceManager.GetMenuActive(MenuName.SettingsMenu)
                && !InterfaceManager.GetMenuActive(MenuName.LobbyMenu)
                )
            {
                var currentPauseMenuStatus = InterfaceManager.GetMenuActive(MenuName.PauseMenu);
                InterfaceManager.TurnOnOff(MenuName.PauseMenu, !currentPauseMenuStatus);
            }
            else
            {
                InterfaceManager.Toggle(MenuName.MainMenu);
            }
        }
    }
}
