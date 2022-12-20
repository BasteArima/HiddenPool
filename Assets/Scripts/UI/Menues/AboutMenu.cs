using TMPro;
using UnityEngine;

public class AboutMenu : BaseMenu
{
    [SerializeField] private TMP_Text _versionText;

    public override void SetData(AppData data)
    {
        base.SetData(data);
        _versionText.text = Application.version;
    }

    public void OnBackButton()
    {
        InterfaceManager.Toggle(MenuName.MainMenu);
    }
}
