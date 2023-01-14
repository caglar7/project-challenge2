using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EventManager : MonoBehaviour
{
    public static event Action GeneratePlatform;    // when player moves to spot and checks

    public static void GeneratePlatformEvent()
    {
        GeneratePlatform?.Invoke();
    }
}
