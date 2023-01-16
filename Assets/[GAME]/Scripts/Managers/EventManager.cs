using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EventManager : MonoBehaviour
{
    // game play events
    public static event Action GeneratePlatform;    // when player moves to spot and checks
    public static event Action<bool> SetInputAvailable;
    public static event Action PlatformStopped;
    public static event Action<Vector3> MovePlayer;
    public static event Action<float> MovePlayerForward;

    // level check condition events
    public static event Action CheckGameCondition;
    public static event Action LevelWin;
    public static event Action LevelFailed;

    public static void GeneratePlatformEvent()
    {
        GeneratePlatform?.Invoke();
    }

    public static void SetInputAvailableEvent(bool value)
    {
        SetInputAvailable?.Invoke(value);
    }

    public static void PlatformStoppedEvent()
    {
        PlatformStopped?.Invoke();
    }

    public static void MovePlayerEvent(Vector3 target)
    {
        MovePlayer?.Invoke(target);
    }

    public static void MovePlayerForwardEvent(float dis)
    {
        MovePlayerForward?.Invoke(dis);
    }

    public static void CheckGameConditionEvent()
    {
        CheckGameCondition?.Invoke();
    }

    public static void LevelWinEvent()
    {
        LevelWin?.Invoke();
    }

    public static void LevelFailedEvent()
    {
        LevelFailed?.Invoke();
    }
}
