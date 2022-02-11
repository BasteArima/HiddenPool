using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class LocalizationSystem : BaseMonoSystem
{
    private static LocalizationSystem _instance;

    private LocalizationObject[] localizationObjects;
    public override void Init(AppData data)
    {
        base.Init(data);
        if (_instance != null) Destroy(_instance.gameObject);
        _instance = this;
    }

    private void Start()
    {
        LocalizeEverything();
    }

    private static void FindLocalizationObjects()
    {
        _instance.localizationObjects = FindObjectsOfType<LocalizationObject>(true);
    }
    private static void InitialLocalizationObjects()
    {
        LocalizationData locData = _instance.data.localizationData;
        foreach (var localizationObject in _instance.localizationObjects)
        {
            if (locData.itemsDict.ContainsKey(localizationObject.Key))
            {
                localizationObject.Init(locData.itemsDict[localizationObject.Key].dict[locData.lang.Value]);
            }
            else
            {
                Debug.LogError($"LocalizationSystem.InitialLocalizationObjects: Key - {localizationObject.Key} doesnt exist in data");
            }
        }
    }

    public static void LocalizeEverything()
    {
        FindLocalizationObjects();
        InitialLocalizationObjects();
    }
    public static void SetLanguage(Language type)
    {
        _instance.data.localizationData.lang.Value = type;
        LocalizeEverything();
    }

    public static string GetText(string key)
    {
        var localizationItem = _instance.data.localizationData.itemsDict[key];
        return localizationItem.dict[_instance.data.localizationData.lang.Value];
    }

    public static Language GetCurrentLang()
    {
        return _instance.data.localizationData.lang.Value;
    }
}
