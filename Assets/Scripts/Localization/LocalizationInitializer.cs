using Assets.SimpleLocalization;
using System;
using UnityEngine;
using UnityEngine.UI;

public class LocalizationInitializer : MonoBehaviour
{
	private const string LANGUAGE = "Language";

	[SerializeField] private Button _rusButton;
	[SerializeField] private Button _engButton;

	private void Awake()
	{
		LocalizationManager.Read();

		if (PlayerPrefs.HasKey(LANGUAGE))
		{
			LocalizationManager.Language = PlayerPrefs.GetString(LANGUAGE);
		}
		else
		{
			switch (Application.systemLanguage)
			{
				case SystemLanguage.Russian:
					LocalizationManager.Language = "Russian";
					break;
				default:
					LocalizationManager.Language = "English";
					break;
			}
		}

		_rusButton?.onClick.AddListener(() => SetLanguage("Russian"));
		_engButton?.onClick.AddListener(() => SetLanguage("English"));
	}

    private void SetLanguage(string language)
    {
		LocalizationManager.Language = language;
		PlayerPrefs.SetString(LANGUAGE, language);
	}
}
