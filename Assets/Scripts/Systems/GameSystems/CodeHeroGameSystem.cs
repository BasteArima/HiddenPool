using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CodeHeroGameSystem : BaseGameSystem
{
    public override void Initialize()
    {

        data.matchData.state.Value = MatchData.State.Game;
    }

    public override void EndGame()
    {
        ClearGame();
        data.matchData.state.Value = MatchData.State.MainMenu;
    }

    private void ClearGame()
    {

    }

    public override void RestartGame()
    {
        ClearGame();
        Initialize();
    }
}
