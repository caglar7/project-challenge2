using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// checking tap inputs, calling proper events
/// </summary>

public class InputManager : MonoBehaviour
{
    bool isInputActive = false;

    private void Start()
    {
        EventManager.SetInputAvailable += SetInputActive;
    }

    private void Update()
    {
        if(isInputActive && Input.GetMouseButtonDown(0))
        {
            isInputActive = false;
            EventManager.PlatformStoppedEvent();
        }
    }

    private void SetInputActive(bool value)
    {
        isInputActive = value;
    }

    private void OnDisable()
    {
        EventManager.SetInputAvailable -= SetInputActive;
    }
}
