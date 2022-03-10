using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HiddenPoolCoreMenu : BaseMenu
{
    public void OnExitGameButton()
    {
        data.matchData.state.Value = MatchData.State.EndGame;
        InterfaceManager.Toggle(MenuName.MainMenu);
    }
}
