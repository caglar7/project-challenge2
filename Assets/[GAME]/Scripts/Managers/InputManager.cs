using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// checking tap inputs, calling proper events
/// </summary>

public class InputManager : MonoBehaviour
{
    #region Properties
    bool isInputActive = false; 
    #endregion

    #region Start, Update
    private void Start()
    {
        EventManager.SetInputAvailable += SetInputActive;
    }

    private void Update()
    {
        if (isInputActive && Input.GetMouseButtonDown(0))
        {
            isInputActive = false;
            EventManager.PlatformStoppedEvent();
        }
    }
    #endregion

    #region Methods
    private void SetInputActive(bool value)
    {
        isInputActive = value;
    }
    #endregion

    #region Disable Listener
    private void OnDisable()
    {
        EventManager.SetInputAvailable -= SetInputActive;
    } 
    #endregion
}
