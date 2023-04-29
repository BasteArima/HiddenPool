using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(TMP_Text))]
public class TMPHyperLinkOpener : MonoBehaviour, IPointerClickHandler
{
    private TMP_Text _textMeshPro;

    private void Start()
    {
        _textMeshPro = GetComponent<TMP_Text>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        int linkIndex = 0;
        #if UNITY_ANDROID || UNITY_IOS || UNITY_IPHONE
        linkIndex = TMP_TextUtilities.FindIntersectingLink(_textMeshPro, Input.GetTouch(0).position, Camera.main);
        #else
        linkIndex = TMP_TextUtilities.FindIntersectingLink(_textMeshPro, Input.mousePosition, Camera.main);
        #endif
        
        if (linkIndex != -1)
        {
            var linkInfo = _textMeshPro.textInfo.linkInfo[linkIndex];
            Application.OpenURL(linkInfo.GetLinkID());
        }
    }
}