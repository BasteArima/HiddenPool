using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ButtonEggSound : MonoBehaviour, IPointerClickHandler
{
    public enum EggPlaySoundInteractables
    {
        Interactable,
        NotInteractable,
        Both
    }

    [SerializeField] private SoundType _interactableSound;
    [SerializeField] private SoundType _notInteractableSound;
    [SerializeField] private SoundType _eggSound;
    [SerializeField] private int _clickAmountToEgg;
    [SerializeField] private EggPlaySoundInteractables _eggPlayOnInteractable;

    private Button _button;
    private int _clickAmount;

    private void Awake()
    {
        _button = GetComponent<Button>();
    }

    private void OnEnable()
    {
        _clickAmount = 0;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        _clickAmount++;
        
        if (_button.interactable && _interactableSound != SoundType.None)
        {
            if ((_eggPlayOnInteractable == EggPlaySoundInteractables.Interactable ||
                 _eggPlayOnInteractable == EggPlaySoundInteractables.Both)
                && _eggSound != SoundType.None)
            {
                if (CheckEggSound())
                    return;
            }

            SoundDesigner.PlaySound(_interactableSound);
        }

        if (!_button.interactable && _notInteractableSound != SoundType.None)
        {
            if ((_eggPlayOnInteractable == EggPlaySoundInteractables.NotInteractable ||
                 _eggPlayOnInteractable == EggPlaySoundInteractables.Both) && _eggSound != SoundType.None)
            {
                if (CheckEggSound())
                    return;
            }

            SoundDesigner.PlaySound(_notInteractableSound);
        }
    }

    private bool CheckEggSound()
    {
        if (_clickAmount < _clickAmountToEgg) return false;

        SoundDesigner.PlaySound(_eggSound);
        _clickAmount = 0;
        return true;
    }
}