using System;
using UnityEngine;

public class BaseMenu : MonoBehaviour
{
    [SerializeField] protected MenuName _name;
    protected bool _state;

    public MenuName Name { get { return _name; } set { _name = value; } }
    public bool State { get { return _state; } set { _state = value; } }
    
    protected virtual Action DoWhenPressEscape { get; }
    public AppData Data => _data;

    protected AppData _data;
    protected InterfaceManager _interfaceManager;
    protected HotKeyInputSystem _hotKeyInputSystem;

    public void Init(InterfaceManager interfaceManager, AppData data, HotKeyInputSystem hotKeyInputSystem)
    {
        _interfaceManager = interfaceManager;
        _data = data;
        _hotKeyInputSystem = hotKeyInputSystem;
    }
    
    public virtual void SetState(bool state)
    {
        _state = state;
    }
    
    protected virtual void OnEnable()
    {
        _hotKeyInputSystem.EscapePressed += DoTargetAction;
    }

    protected virtual void OnDisable()
    {
        _hotKeyInputSystem.EscapePressed -= DoTargetAction;
    }
    
    private void DoTargetAction()
    {
        DoWhenPressEscape?.Invoke();
    }
}


