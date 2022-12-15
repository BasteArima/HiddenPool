using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.SimpleLocalization
{
	/// <summary>
	/// Localize text component.
	/// </summary>
    [RequireComponent(typeof(TMP_Text))]
    public class LocalizedText : MonoBehaviour
    {
        public string LocalizationKey;
        private TMP_Text _text;

        public void Start()
        {
            _text = GetComponent<TMP_Text>();
            Localize();
            LocalizationManager.LocalizationChanged += Localize;
        }

        public void OnDestroy()
        {
            LocalizationManager.LocalizationChanged -= Localize;
        }

        private void Localize()
        {
            _text.text = LocalizationManager.Localize(LocalizationKey);
        }
    }
}