using UnityEngine;

public class BaseMenu : MonoBehaviour
{
    [SerializeField] protected MenuName _name;
    protected bool _state;

    public MenuName Name { get { return _name; } set { _name = value; } }
    public bool State { get { return _state; } set { _state = value; } }
    public AppData Data => data;

    protected AppData data;

    public virtual void SetState(bool state)
    {
        _state = state;
    }

    public virtual void SetData(AppData data)
    {
        this.data = data;
    }
}


