using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HotKeyInputSystem : BaseMonoSystem
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!InterfaceManager.GetMenuActive(MenuName.MainMenu) && !InterfaceManager.GetMenuActive(MenuName.ChoiseGameMenu))
            {
                var currentPauseMenuStatus = InterfaceManager.GetMenuActive(MenuName.PauseMenu);
                InterfaceManager.TurnOnOff(MenuName.PauseMenu, !currentPauseMenuStatus);
            }
        }
    }
}
