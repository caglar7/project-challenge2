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

    Vector3 nextSpawnPos;

    private void Start()
    {
        nextSpawnPos = startPoint.position;
        EventManager.GeneratePlatform += GeneratePlatform;
    }

    /// <summary>
    /// called with generate event
    /// </summary>
    private void GeneratePlatform()
    {
        GameObject platform = PoolManager.instance.platformPool.PullObjFromPool();
        platform.GetComponent<PlatformMover>().StartMoving(GetSpawnPos(), 1f);

        // updatenext pos after generation
    }

    private Vector3 GetSpawnPos()
    {
        Vector3 pos = nextSpawnPos;
        nextSpawnPos.x = -nextSpawnPos.x;
        nextSpawnPos.z += zDiff;
        return pos;
    }
}
