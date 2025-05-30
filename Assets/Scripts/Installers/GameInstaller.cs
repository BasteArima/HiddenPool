using UnityEngine;
using Zenject;

public class GameInstaller : MonoInstaller
{
    [SerializeField] private AppData _data;
    [SerializeField] private InterfaceManager _interfaceManager;
    [SerializeField] private HotKeyInputSystem _hotKeyInputSystem;
    [SerializeField] private CardsGenerateSystem _cardsGenerateSystem;
    
    public override void InstallBindings()
    {
        Container.Bind<AppData>().FromInstance(_data).AsCached().NonLazy();
        Container.Bind<InterfaceManager>().FromInstance(_interfaceManager).AsCached().NonLazy();
        Container.Bind<HotKeyInputSystem>().FromInstance(_hotKeyInputSystem).AsCached().NonLazy();
        Container.Bind<CardsGenerateSystem>().FromInstance(_cardsGenerateSystem).AsCached().NonLazy();
        
        _data.matchData.state.Value = MatchData.State.MainMenu;
    }
}