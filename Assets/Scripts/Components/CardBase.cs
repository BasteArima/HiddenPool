using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using DG.Tweening;
using System.Collections;
using System;

public class CardBase : MonoBehaviour, IPointerClickHandler
{
    public enum CardStatuses { None, Closed, Maybe }

    [Header("Settings")]
    [SerializeField] private float _choiseCardColorSpeed = 0.5f;

    [Header("Hovers")]
    [SerializeField] private GameObject _disableHoverImg;
    [SerializeField] private GameObject _maybeHoverImg;
    [SerializeField] private Image _choisedHoverImg;

    [Header("Elements")]
    [SerializeField] private Image _cardImg;
    [SerializeField] private Image _cardAttribute;
    [SerializeField] private TMP_Text _cardName;
    [SerializeField] private Sprite[] _attributeSprites;

    [SerializeField] private CardStatuses _cardStatus = CardStatuses.None;
    private int _clickCount = 0;
    private CardsGenerateSystem _cardsGenerateSystem;

    public Sprite CardSprite => _cardImg.sprite;

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

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
            OnCardClick();
        else if (eventData.button == PointerEventData.InputButton.Middle)
            ClearCard();
        else if (eventData.button == PointerEventData.InputButton.Right)
            StartCoroutine(ChoiseMainCard());
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

    private IEnumerator ChoiseMainCard()
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
