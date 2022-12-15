using UnityEngine;

[CreateAssetMenu(menuName = "Data/AppData")]
public class AppData : ScriptableObject
{
    public UserData userData;
    public MatchData matchData;
    public HeroesData heroesData;
}
