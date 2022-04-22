using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChoiseGameMenu : BaseMenu
{
    [SerializeField] private Image _pashalkaImage;
    [SerializeField] private Sprite[] _pashalkaSprite;

    private int _runoMessClickCount = 0;
    private List<SoundType> _runeSoundTypes = new List<SoundType> { SoundType.RuneArcane, SoundType.RuneBounty, SoundType.RuneDD,
            SoundType.RuneHaste, SoundType.RuneInvisible, SoundType.RuneRegen, SoundType.RuneWater, SoundType.RuneIllusion };

    public override void SetState(bool state)
    {
        base.SetState(state);
        if (state)
        {
            _pashalkaImage.gameObject.SetActive(false);
            _runoMessClickCount = 0;
        }
    }

    public void OnHiddenPoolButton()
    {
        //NetworkManager.Singleton.StartClient();
        data.matchData.state.Value = MatchData.State.InitializeGame;
        ChooseGameSystem.ChooseGame(MatchData.MiniGames.HiddenPool);
        InterfaceManager.Toggle(MenuName.HiddenPoolCoreMenu);
    }

    public void OnRunoMessButton()
    {
        var random = Random.Range(0, 8);
        SoundDesigner.PlaySound(_runeSoundTypes[random]);

        _runoMessClickCount++;
        if (_runoMessClickCount > 5)
        {
            _pashalkaImage.gameObject.SetActive(true);
            var rand = Random.Range(0, _pashalkaSprite.Length);
            _pashalkaImage.sprite = _pashalkaSprite[rand];
            _runoMessClickCount = 0;
        }
    }

    public void OnPashalkaButton()
    {
        _pashalkaImage.gameObject.SetActive(false);
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
        //NetworkManager.Singleton.StartClient();
        data.matchData.state.Value = MatchData.State.InitializeGame;
        ChooseGameSystem.ChooseGame(MatchData.MiniGames.CodeHero);
        InterfaceManager.Toggle(MenuName.CodeHeroCoreMenu);
    }

    public void OnBackButton()
    {
        InterfaceManager.Toggle(MenuName.MainMenu);
    }
}
