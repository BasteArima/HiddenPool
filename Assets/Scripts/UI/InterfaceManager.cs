using System.Linq;
using TMPro;
using UnityEngine;

public enum MenuName
{
    None, MainMenu, HiddenPoolCoreMenu, AboutMenu, SettingsMenu, ChoiseGameMenu, RunoMessCoreMenu, TeaEyeWinnerCoreMenu, ShockContentCoreMenu, MemeHistoryCoreMenu, MenuQuizCoreMenu, Locked1, Locked2, PauseMenu, CodeHeroCoreMenu, LobbyMenu
}
public class InterfaceManager : BaseMonoSystem
{
    private static InterfaceManager _instance;

    [SerializeField] private BaseMenu[] _menus;

    [SerializeField] private TMP_Text _versionText;

    public override void Init(AppData data)
    {
        base.Init(data);

        if (_instance != null) Destroy(_instance.gameObject);
        _instance = this;

        AddDataForAllBaseMenu();
        Toggle(MenuName.MainMenu);
        Application.targetFrameRate = 60;
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        Time.timeScale = 1;
    }

    private void Start()
    {
        _versionText.text = Application.version;
        SoundDesigner.PlaySound(SoundType.MainMenuLoop);
    }

    private void AddDataForAllBaseMenu()
    {
        foreach (var baseMenu in _menus)
        {
            baseMenu.SetData(data);
        }
    }

    public static void Toggle(MenuName name)
    {
        foreach (var baseMenu in _instance._menus)
        {
            var state = baseMenu.Name == name;
            baseMenu.gameObject.SetActive(state);
            baseMenu.SetState(state);
        }
    }

    public static void TurnOnOff(MenuName name, bool state)
    {
        var baseMenu = _instance._menus.SingleOrDefault(m => m.Name == name);
        if (baseMenu == null) return;
        baseMenu.gameObject.SetActive(state);
        baseMenu.SetState(state);
    }

    public static bool GetMenuActive(MenuName name)
    {
        var baseMenu = _instance._menus.SingleOrDefault(m => m.Name == name);
        if (baseMenu == null) return false;
        return baseMenu.State;
    }
    
    [SerializeField] private TMP_Text[] _texts;
    [SerializeField] private TMP_FontAsset _font;

    [ContextMenu("Test")]
    private void Test()
    {
        _texts = FindObjectsOfType<TMP_Text>(true);
        foreach (var text in _texts)
            text.font = _font;
    }
}
