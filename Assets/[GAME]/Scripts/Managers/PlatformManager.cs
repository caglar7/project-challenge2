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
    [SerializeField] float zDiff;
    [SerializeField] Transform parentObject;

    [Header("Finish Platform Settings")]
    [SerializeField] Transform firstBlockPoint;
    [SerializeField] Transform finishPlatform;
    [Range(1, 20)][SerializeField] int blocksToFinish;
    int generatedPlatformCount;

    [Header("Fields")]
    Vector3 nextSpawnPos;
    Vector3 nextScale;
    PlatformObject currentPlatform;
    PlatformMover currentMover;
    Slicer meshSlicer;

    bool initPlatformSet = false;
    public static int currentLevel;

    private void Awake()
    {
        meshSlicer = GetComponent<Slicer>();
    }

    private void Start()
    {
        nextSpawnPos = startPoint.position;
        SetFinishPosition();
        currentLevel++;
        LevelCount.instance.tmpro.text = "Level " + currentLevel.ToString();
        SetFirstPlatformScale();

        EventManager.GeneratePlatform += GeneratePlatform;
        EventManager.PlatformStopped += StopCurrentPlatform;
        EventManager.LevelWin += SaveScaleXData;
        EventManager.LevelFailed += ResetValues;
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
        SetInitPlatformScale(platform);

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

            // slice, get scale for next object spawn
            nextScale = meshSlicer.SliceObject(currentPlatform.gameObject);
            Vector3 playerNextPos = meshSlicer.RemainingObjectPosition();

            if (playerNextPos == Vector3.zero) EventManager.MovePlayerForwardEvent(zDiff);
            else EventManager.MovePlayerEvent(playerNextPos);
        }
    }

    private void SetFinishPosition()
    {
        finishPlatform.position = firstBlockPoint.position + new Vector3(0f, 0f, (zDiff * blocksToFinish));
    }

    private void OnDisable()
    {
        EventManager.GeneratePlatform -= GeneratePlatform;
        EventManager.PlatformStopped -= StopCurrentPlatform;
        EventManager.LevelWin -= SaveScaleXData;
    }

    /// <summary>
    /// called after set level count
    /// </summary>
    private void SetInitPlatformScale(GameObject g)
    {
        if(initPlatformSet == false && currentLevel > 1)
        {
            initPlatformSet = true;
            Vector3 scale = g.transform.localScale;
            scale.x = PlayerPrefs.GetFloat("ScaleX");
            g.transform.localScale = scale;
            firstBlockPoint.localScale = scale;
        }
    }

    private void SetFirstPlatformScale()
    {
        if(currentLevel > 1)
        {
            Vector3 scale = firstBlockPoint.localScale;
            scale.x = PlayerPrefs.GetFloat("ScaleX");
            firstBlockPoint.localScale = scale;
        }
    }

    private void SaveScaleXData()
    {
        PlayerPrefs.SetFloat("ScaleX", nextScale.x);
    }

    private void ResetValues()
    {
        GameObject clone = PoolManager.instance.platformPool.PullObjFromPool();
        float scaleX = clone.transform.localScale.x;
        PlayerPrefs.SetFloat("ScaleX", scaleX);
        PoolManager.instance.platformPool.AddObjToPool(clone);
        currentLevel = 0;
    }
}
