using UnityEngine;
using UniRx;

[CreateAssetMenu(menuName = "Data/UserData")]
public class UserData : ScriptableObject
{
    public StringReactiveProperty userName;
    public Texture2D userAvatar;
    public Sprite userSprite;
    public IntReactiveProperty sessionsCount;
    public IntReactiveProperty coins;
    public IntReactiveProperty wins;

    public void ClearData()
    {
        userName.Value = "";
        sessionsCount.Value = 0;
        coins.Value = 0;
        wins.Value = 0;
        PlayerPrefs.DeleteAll();
    }
}
