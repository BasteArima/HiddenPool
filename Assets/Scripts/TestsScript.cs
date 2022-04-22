using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TestsScript : MonoBehaviour
{
    [SerializeField] private Toggle _toggle;

    private void Awake()
    {
        //_toggle.onValueChanged.AddListener(OnToggleChanged);
    }

    public void OnToggleChanged(bool active)
    {
        gameObject.SetActive(active);
    }

    private IEnumerator Test()
    {
        yield return new WaitForEndOfFrame();
    }
}
