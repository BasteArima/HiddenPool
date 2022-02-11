using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CardBase : MonoBehaviour
{
    [SerializeField] private GameObject _disableHoverImg;
    [SerializeField] private Image _cardImg;
    [SerializeField] private Image _cardAttribute;
    [SerializeField] private TMP_Text _cardName;

    [SerializeField] private Sprite[] _attributeSprites;

    public Sprite CardSprite => _cardImg.sprite;

    public void SetData(Sprite cardSprite, string cardName = "", AttributeTypes heroAtribute = AttributeTypes.None)
    {
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

    /// <summary>
    /// if true, card is active
    /// </summary>
    /// <param name="enabled"></param>
    public void SetStatus(bool enabled)
    {
        _disableHoverImg.SetActive(!enabled);
    }
}
