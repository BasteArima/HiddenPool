using System.Collections.Generic;
using UnityEngine;

public class AboutMenu : BaseMenu
{
    public override void SetState(bool state)
    {
        base.SetState(state);
        if (state)
        {
        }
    }

    public void OnBackButton()
    {
        InterfaceManager.Toggle(MenuName.MainMenu);
    }
}
