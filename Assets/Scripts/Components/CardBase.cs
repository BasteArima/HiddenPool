using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using DG.Tweening;
using System.Collections;
using System;

public class CardBase : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler
{
    public enum CardStatuses { None, Closed, Maybe }

    [Header("Settings")]
    [SerializeField] private float _choiseCardColorSpeed = 0.5f;
    [SerializeField] private float _pointDownTimeToStartChoice = 1f;
    [SerializeField] private float _pointDownTimeToChoice = 4f;

    [Header("Hovers")]
    [SerializeField] private GameObject _disableHoverImg;
    [SerializeField] private GameObject _maybeHoverImg;
    [SerializeField] private Image _choisedHoverImg;
    [SerializeField] private Image _choisingHoverImg;

    [Header("Elements")]
    [SerializeField] private Image _cardImg;
    [SerializeField] private Image _cardAttribute;
    [SerializeField] private TMP_Text _cardName;
    [SerializeField] private Sprite[] _attributeSprites;

    [SerializeField] private CardStatuses _cardStatus = CardStatuses.None;
    private int _clickCount = 0;
    private CardsGenerateSystem _cardsGenerateSystem;
    private RectTransform _rectTransform;
    private bool _pointDown;
    private float _timePointDown;
    private float _timePointFill;
    
    public Sprite CardSprite => _cardImg.sprite;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
    }

    public void SetData(CardsGenerateSystem cardGenSystem, Sprite cardSprite, string cardName = "", AttributeTypes heroAtribute = AttributeTypes.None)
    {
        _cardsGenerateSystem = cardGenSystem;
        _cardImg.sprite = cardSprite;
        _cardName.text = cardName;
        SetHeroAttribute(heroAtribute);
    }

    private void SetHeroAttribute(AttributeTypes heroAtribute)
    {
        if (heroAtribute == AttributeTypes.None) return;
        
        _cardAttribute.gameObject.SetActive(true);

        _cardAttribute.sprite = _attributeSprites[(int)heroAtribute + 1];
    }

    private void Update()
    {
        if (!_pointDown) return;

        _timePointDown += Time.deltaTime;
        if (_timePointDown >= _pointDownTimeToStartChoice)
        {
            _timePointFill += Time.deltaTime;
            _choisingHoverImg.gameObject.SetActive(true);
            _choisingHoverImg.fillAmount = _timePointFill / _pointDownTimeToChoice;
            if (_timePointFill >= _pointDownTimeToChoice)
            {
                var pointer = new PointerEventData(EventSystem.current);
                ExecuteEvents.Execute(gameObject, pointer, ExecuteEvents.pointerUpHandler); // [TODO] Fix double event
                ChooseOpponentCard();
            }
        }
    }

    private void ChooseOpponentCard() // [TODO] Add logic for multiplayer
    {
        Debug.Log($"ChooseOpponentCard");
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        _rectTransform.DOScale(0.9f, 0.1f).OnComplete(() => _pointDown = true);
    }
    
    public void OnPointerUp(PointerEventData eventData)
    {
        Debug.Log($"OnPointerUp");

        _pointDown = false;
        _choisingHoverImg.gameObject.SetActive(false);
        _choisingHoverImg.fillAmount = 0;
        
        var seq = DOTween.Sequence();
        seq.SetUpdate(true);
        seq.Append(_rectTransform.DOScale(1.1f, 0.08f));
        seq.Append(_rectTransform.DOScale(1f, 0.08f));

        if (_timePointDown < _timePointFill)
        {
            if (eventData.button == PointerEventData.InputButton.Left)
                OnCardClick();
            else if (eventData.button == PointerEventData.InputButton.Middle)
                ClearCard();
            else if (eventData.button == PointerEventData.InputButton.Right)
                StartCoroutine(ChoiceMainCard());
        }

        _timePointDown = 0;
        _timePointFill = 0;
    }
    
    public void OnPointerClick(PointerEventData eventData)
    {

    }

    private void OnCardClick()
    {
        _clickCount++;

        if (_clickCount > Enum.GetNames(typeof(CardStatuses)).Length - 1)
            _clickCount = 0;

        SetStatus((CardStatuses)_clickCount);
    }

    private void SetStatus(CardStatuses status = CardStatuses.None)
    {
        _cardStatus = status;

        switch (status)
        {
            case CardStatuses.None:
                _disableHoverImg.SetActive(false);
                _maybeHoverImg.SetActive(false);
                break;
            case CardStatuses.Closed:
                _disableHoverImg.SetActive(true);
                _maybeHoverImg.SetActive(false);
                break;
            case CardStatuses.Maybe:
                _disableHoverImg.SetActive(false);
                _maybeHoverImg.SetActive(true);
                break;
        }
    }

    private IEnumerator ChoiceMainCard()
    {
        if (_cardsGenerateSystem.MainCardIsLocked) yield break;

        _cardsGenerateSystem.SetMainCard(_cardImg.sprite);
        _choisedHoverImg.gameObject.SetActive(true);
        Tween myTween = _cardImg.DOColor(Color.green, _choiseCardColorSpeed);
        yield return myTween.WaitForCompletion();
        _cardImg.DOColor(Color.white, _choiseCardColorSpeed);
        yield return myTween.WaitForCompletion();
        _choisedHoverImg.gameObject.SetActive(false);
    }

    public void ClearCard()
    {
        _clickCount = 0;
        SetStatus((CardStatuses)_clickCount);
    }
}
