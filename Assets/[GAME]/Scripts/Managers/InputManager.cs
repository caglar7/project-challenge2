using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// checking tap inputs, calling proper events
/// </summary>

public class InputManager : MonoBehaviour
{
    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            EventManager.GeneratePlatformEvent();
        }
    }
}
