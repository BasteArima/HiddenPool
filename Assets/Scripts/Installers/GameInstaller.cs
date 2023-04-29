using UnityEngine;
using Zenject;

public class GameInstaller : MonoInstaller
{
    [SerializeField] private AppData _data;
    [SerializeField] private InterfaceManager _interfaceManager;
    [SerializeField] private ChooseGameNetworkManager _chooseGameNetworkManager;
    [SerializeField] private HotKeyInputSystem _hotKeyInputSystem;
    
    public override void InstallBindings()
    {
        Container.Bind<AppData>().FromInstance(_data).AsCached().NonLazy();
        Container.Bind<InterfaceManager>().FromInstance(_interfaceManager).AsCached().NonLazy();
        Container.Bind<ChooseGameNetworkManager>().FromInstance(_chooseGameNetworkManager).AsCached().NonLazy();
        Container.Bind<HotKeyInputSystem>().FromInstance(_hotKeyInputSystem).AsCached().NonLazy();
        
        _data.matchData.state.Value = MatchData.State.MainMenu;
        _data.matchData.game.Value = MatchData.MiniGames.None;
    }
}