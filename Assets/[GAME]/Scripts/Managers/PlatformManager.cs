using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Platform generating, slicing methods
/// </summary>

public class PlatformManager : MonoBehaviour
{
    [Header("Spawn Settings")]
    [SerializeField] Transform startPoint;
    [SerializeField] float zDiff = 1f;
    [SerializeField] Transform parentObject;

    [Header("Fields")]
    Vector3 nextSpawnPos;
    PlatformObject currentPlatform;
    PlatformMover currentMover;

    private void Start()
    {
        nextSpawnPos = startPoint.position;
        EventManager.GeneratePlatform += GeneratePlatform;
        EventManager.PlatformStopped += StopCurrentPlatform;
    }

    /// <summary>
    /// called with generate event
    /// </summary>
    private void GeneratePlatform()
    {
        GameObject platform = PoolManager.instance.platformPool.PullObjFromPool();
        currentPlatform = platform.GetComponent<PlatformObject>();
        currentMover = platform.GetComponent<PlatformMover>();
        platform.transform.SetParent(parentObject);

        currentMover.StartMoving(GetSpawnPos(), 1f);
    }

    /// <summary>
    /// get next position, update next position for spawning
    /// </summary>
    /// <returns></returns>
    private Vector3 GetSpawnPos()
    {
        Vector3 pos = nextSpawnPos;
        nextSpawnPos.x = -nextSpawnPos.x;
        nextSpawnPos.z += zDiff;
        return pos;
    }

    /// <summary>
    /// on input tap, stop the currently moving platform
    /// </summary>
    private void StopCurrentPlatform()
    {
        if(currentMover)
        {
            currentMover.StopMoving();

            // slice here and give a pos for player to go
            // if slice fails and cube fall, player just go +zDiff

            EventManager.MovePlayerEvent(currentPlatform.transform.position);
        }
    }
}
