using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(TextMeshProUGUI))]
public class LocalizationObject : MonoBehaviour
{
    [SerializeField]
    private string key;
    private TextMeshProUGUI textComponent;

    public string Key
    {
        get { return key; }
        set { key = value; }
    }

    public void Init(string value)
    {
        textComponent = GetComponent<TextMeshProUGUI>();

        textComponent.text = value;
    } 
}
