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

    [Header("Finish Platform Settings")]
    [SerializeField] Transform firstBlockPoint;
    [SerializeField] Transform finishPlatform;
    [Range(1, 20)][SerializeField] int blocksToFinish = 10;
    int generatedPlatformCount = 1;

    [Header("Fields")]
    Vector3 nextSpawnPos;
    Vector3 nextScale;
    PlatformObject currentPlatform;
    PlatformMover currentMover;
    Slicer meshSlicer;

    private void Awake()
    {
        meshSlicer = GetComponent<Slicer>();
    }

    private void Start()
    {
        nextSpawnPos = startPoint.position;
        SetFinishPosition();
        EventManager.GeneratePlatform += GeneratePlatform;
        EventManager.PlatformStopped += StopCurrentPlatform;
    }

    /// <summary>
    /// called with generate event
    /// </summary>
    private void GeneratePlatform()
    {
        if (generatedPlatformCount >= blocksToFinish) return;

        GameObject platform = PoolManager.instance.platformPool.PullObjFromPool();
        currentPlatform = platform.GetComponent<PlatformObject>();
        currentMover = platform.GetComponent<PlatformMover>();
        platform.transform.SetParent(parentObject);
        platform.transform.localScale = (nextScale != Vector3.zero) ? nextScale : platform.transform.localScale;

        currentMover.StartMoving(GetSpawnPos(), 1f);
        generatedPlatformCount++;
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
        if (generatedPlatformCount >= blocksToFinish) return;

        if (currentMover)
        {
            currentMover.StopMoving();

            // slice here and give a pos for player to go
            // if slice fails and entire cube falls, player just go +zDiff

            // slice, get scale for next object spawn
            nextScale = meshSlicer.SliceObject(currentPlatform.gameObject);
            Vector3 playerNextPos = meshSlicer.RemainingObjectPosition();

            if (playerNextPos == Vector3.zero) EventManager.MovePlayerForwardEvent(zDiff);
            else EventManager.MovePlayerEvent(playerNextPos);
        }
    }

    private void SetFinishPosition()
    {
        finishPlatform.position = firstBlockPoint.position + new Vector3(0f, 0f, (zDiff * (blocksToFinish - 1)));
    }
}
