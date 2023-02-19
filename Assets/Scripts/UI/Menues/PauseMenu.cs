using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : BaseMenu
{
    [SerializeField] private Button _continueButton;
    [SerializeField] private Button _exitButton;

    private void Awake()
    {
        _continueButton.onClick.AddListener(OnContinueButton);
        _exitButton.onClick.AddListener(OnExitToMenuButton);
    }

    private void OnContinueButton()
    {
        InterfaceManager.TurnOnOff(MenuName.PauseMenu, false);
    }

    private void OnExitToMenuButton()
    {
        CustomNetworkManager.Instance.StopHost();
        //FirebaseController.Instance.RoomExit();
    }
}