using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ConfirmWindowView : MonoBehaviour
{
    public UnityAction ActionConfirmed;
    
    [SerializeField] private Transform _mainPanel;
    [SerializeField] private Image _background;
    [SerializeField] private Button _yesButton;
    [SerializeField] private Button _noButton;

    private void Start()
    {
        _yesButton.onClick.AddListener(OnYesButton);
        _noButton.onClick.AddListener(OnNoButton);
    }
    
    private void OnEnable()
    {
        _mainPanel.DOScale(1, 0.45f).From(0).SetEase(Ease.OutBack).SetUpdate(true);
        _background.DOFade(0.7f, 0.45f).From(0).SetUpdate(true);
    }

    private void OnYesButton()
    {
        ActionConfirmed?.Invoke();
    }

    private void OnNoButton()
    {
        var seq = DOTween.Sequence();
        seq.SetUpdate(true);
        seq.Append(_mainPanel.DOScale(0, 0.37f));
        seq.Append(_background.DOFade(0, 0.37f));
        seq.OnComplete(() => { gameObject.SetActive(false); });
    }
    
    public void ConfirmAction(UnityAction actionResult)
    {
        ActionConfirmed = actionResult;
    }
}
