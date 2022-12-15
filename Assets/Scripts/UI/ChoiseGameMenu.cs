using System.Collections.Generic;
using UnityEngine;

public class ChoiseGameMenu : BaseMenu
{
    private List<SoundType> _runeSoundTypes = new List<SoundType> { SoundType.RuneArcane, SoundType.RuneBounty, SoundType.RuneDD,
            SoundType.RuneHaste, SoundType.RuneInvisible, SoundType.RuneRegen, SoundType.RuneWater, SoundType.RuneIllusion };

    public override void SetState(bool state)
    {
        base.SetState(state);
        if (state)
        {
        }
    }

    public void OnHiddenPoolButton()
    {
        data.matchData.state.Value = MatchData.State.InitializeGame;
        ChooseGameSystem.ChooseGame(MatchData.MiniGames.HiddenPool);
        InterfaceManager.Toggle(MenuName.HiddenPoolCoreMenu);
    }

    public void OnRunoMessButton()
    {
        var random = Random.Range(0, _runeSoundTypes.Count);
        Debug.Log("Random: " + random);
        SoundDesigner.PlaySound(_runeSoundTypes[random]);
    }

    public void OnTeaEyeWinnerButton()
    {

    }

    public void OnShockContentButton()
    {

    }

    public void OnMemeHistoryButton()
    {

    }

    public void OnCodeHeroButton()
    {
        data.matchData.state.Value = MatchData.State.InitializeGame;
        ChooseGameSystem.ChooseGame(MatchData.MiniGames.CodeHero);
        InterfaceManager.Toggle(MenuName.CodeHeroCoreMenu);
    }

    public void OnBackButton()
    {
        InterfaceManager.Toggle(MenuName.MainMenu);
    }
}
