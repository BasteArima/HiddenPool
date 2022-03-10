using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChoiseGameMenu : BaseMenu
{
    [SerializeField] private Image _pashalkaImage;
    [SerializeField] private Sprite[] _pashalkaSprite;

    private int _runoMessClickCount = 0;

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
        switch (random)
        {
            case 0:
                SoundDesigner.PlaySound(SoundType.RuneArcane);
                break;
            case 1:
                SoundDesigner.PlaySound(SoundType.RuneBounty);
                break;
            case 2:
                SoundDesigner.PlaySound(SoundType.RuneDD);
                break;
            case 3:
                SoundDesigner.PlaySound(SoundType.RuneHaste);
                break;
            case 4:
                SoundDesigner.PlaySound(SoundType.RuneInvisible);
                break;
            case 5:
                SoundDesigner.PlaySound(SoundType.RuneRegen);
                break;
            case 6:
                SoundDesigner.PlaySound(SoundType.RuneWater);
                break;
            case 7:
                SoundDesigner.PlaySound(SoundType.RuneIllusion);
                break;
            default:
                break;
        }

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
