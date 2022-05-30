using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/HeroesData")]
public class HeroesData : ScriptableObject
{
    public HeroData[] heroes;
    public Sprite[] heroesSprites;
    public Sprite[] itemsSprites;
    public Sprite[] neutralItems;
    public Sprite[] mobs;
}

[System.Serializable]
public struct HeroData
{
    public Sprite cardImage;
    public string name;
    public AttributeTypes attribute;
}

public enum AttributeTypes { None = 0, Strength, Agility, Intelligence }