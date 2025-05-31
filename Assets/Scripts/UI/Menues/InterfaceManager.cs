using System.Linq;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

public enum MenuName
{
    None = 0,
    MainMenu = 1,
    HiddenPoolCoreMenu = 2,
    AboutMenu = 3,
    SettingsMenu = 4,
    PauseMenu = 5,
}

public class InterfaceManager : MonoBehaviour
{
    [SerializeField] private BaseMenu[] _menus;
    [SerializeField] private TMP_Text _versionText;
    [SerializeField] private AppData _data;
    [SerializeField] private HotKeyInputSystem _hotKeyInputSystem;

    private void Awake()
    {
        foreach (var menu in _menus)
            menu.Init(this, _data, _hotKeyInputSystem);
    }

    private void Start()
    {
        Toggle(MenuName.MainMenu);
        _versionText.text = $"v{Application.version}";
        SoundDesigner.PlaySound(SoundType.MainMenuLoop);
    }

    public void Toggle(MenuName name)
    {
        foreach (var baseMenu in _menus)
        {
            var state = baseMenu.Name == name;
            baseMenu.gameObject.SetActive(state);
            baseMenu.SetState(state);
        }
    }

    public void TurnOnOff(MenuName name, bool state)
    {
        var baseMenu = _menus.SingleOrDefault(m => m.Name == name);
        if (baseMenu == null) return;
        baseMenu.gameObject.SetActive(state);
        baseMenu.SetState(state);
    }

    public bool GetMenuActive(MenuName name)
    {
        var baseMenu = _menus.SingleOrDefault(m => m.Name == name);
        if (baseMenu == null) return false;
        return baseMenu.State;
    }

    #if UNITY_EDITOR
    [SerializeField] private TMP_FontAsset _font;

    [Button]
    private void ChangeFont()
    {
        var texts = FindObjectsOfType<TMP_Text>(true);
        foreach (var text in texts)
            text.font = _font;
    }
    #endif
}