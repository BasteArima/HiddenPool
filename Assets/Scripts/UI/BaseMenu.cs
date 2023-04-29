using System;
using UnityEngine;
using Zenject;

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

    [Inject]
    protected virtual void Construct(AppData data, InterfaceManager interfaceManager,
        HotKeyInputSystem hotKeyInputSystem)
    {
        _data = data;
        _interfaceManager = interfaceManager;
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


