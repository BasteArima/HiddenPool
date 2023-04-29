using System;
using UnityEngine;

public class HotKeyInputSystem : MonoBehaviour
{
    public Action EscapePressed;
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            EscapePressed?.Invoke();
    }
}
