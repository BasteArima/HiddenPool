using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using DG.Tweening;
using System.Collections;
using System;

public class CardBase : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private enum CardStatuses
    {
        None,
        Closed,
        Maybe
    }

    [Header("Settings")]
    [SerializeField] private float _choiseCardColorSpeed = 0.5f;

    [Header("Hovers")]
    [SerializeField] private GameObject _disableHoverImg;
    [SerializeField] private GameObject _maybeHoverImg;
    [SerializeField] private Image _choisedHoverImg;

    [Header("Elements")] [SerializeField] private Image _cardImg;
    [SerializeField] private TMP_Text _cardName;
    [SerializeField] private CardStatuses _cardStatus = CardStatuses.None;

    private int _clickCount = 0;
    private CardsGenerateSystem _cardsGenerateSystem;
    private RectTransform _rectTransform;

    public Sprite CardSprite => _cardImg.sprite;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
    }

    public void SetData(CardsGenerateSystem cardGenSystem, Sprite cardSprite, string cardName = "")
    {
        _cardsGenerateSystem = cardGenSystem;
        _cardImg.sprite = cardSprite;
        _cardName.text = cardName;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        _rectTransform.DOScale(0.9f, 0.1f);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        var seq = DOTween.Sequence();
        seq.SetUpdate(true);
        seq.Append(_rectTransform.DOScale(1.1f, 0.08f));
        seq.Append(_rectTransform.DOScale(1f, 0.08f));

        switch (eventData.button)
        {
            case PointerEventData.InputButton.Left:
                OnCardClick();
                break;
            case PointerEventData.InputButton.Middle:
                ClearCard();
                break;
            case PointerEventData.InputButton.Right:
                StartCoroutine(ChoiceMainCard());
                break;
        }
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
        var myTween = _cardImg.DOColor(Color.green, _choiseCardColorSpeed);
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