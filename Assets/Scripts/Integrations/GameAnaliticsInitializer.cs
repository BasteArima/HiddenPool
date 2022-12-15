using GameAnalyticsSDK;
using UnityEngine;

public class GameAnaliticsInitializer : MonoBehaviour
{
    private void Start()
    {
        GameAnalytics.Initialize();
    }
}
