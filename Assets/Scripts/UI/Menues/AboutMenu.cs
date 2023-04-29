using System;
using TMPro;
using UnityEngine;

public class AboutMenu : BaseMenu
{
    [SerializeField] private TMP_Text _versionText;

    protected override Action DoWhenPressEscape => OnBackButton;
    
    private void Awake()
    {
        _versionText.text = Application.version;
    }

    public void OnBackButton()
    {
        _interfaceManager.Toggle(MenuName.MainMenu);
    }
}
