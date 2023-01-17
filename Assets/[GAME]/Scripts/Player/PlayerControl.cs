using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///  player controller
/// </summary>

public class PlayerControl : MonoBehaviour
{
    #region Properties
    [Header("Initial Settings")]
    [SerializeField] Transform initTarget;

    [Header("Fail Settings")]
    [SerializeField] float jumpPower;
    [SerializeField] float jumpDuration;
    [SerializeField] float fallDistance;

    [Header("Fields")]
    PlayerMover mover;
    float zDiffValue;
    #endregion

    #region Awake, Start
    private void Awake()
    {
        mover = GetComponent<PlayerMover>();
    }

    private void Start()
    {
        MoveToPosition(initTarget.position);
        SetDiffZ();
        EventManager.MovePlayer += MoveToPosition;
        EventManager.MovePlayerForward += MoveForward;
        EventManager.CheckGameCondition += CheckGame;
    }
    #endregion

    #region Move Methods
    private void MoveToPosition(Vector3 pos)
    {
        mover.Move(pos);
    }

    private void MoveForward(float dis)
    {
        Vector3 pos = transform.position;
        pos.z += dis;
        mover.Move(pos);
    }
    #endregion

    #region Check Game Method
    /// <summary>
    /// game conditions when player reached to next spot 
    /// </summary>
    private void CheckGame()
    {
        PlatformObject platformObject = CheckPlatform();
        FinishPlatform finishPlatform = CheckFinishPlatform();
        FinishPlatform finishPlatformFront = CheckFinishPlatformFront();

        if (platformObject && finishPlatformFront)                               // next block is finish
        {
            mover.Move(transform.position + new Vector3(0f, 0f, zDiffValue));
            Debug.Log("Finish is next");
        }
        else if (finishPlatform)                                                 // finish, level done
        {
            mover.PlayAnimation(AnimationType.Dance);
            CanvasController.instance.SwitchCanvas(CanvasType.WinMenu);
            EventManager.LevelWinEvent();
            Debug.Log("Level Win");
        }
        else if (platformObject)                                                 // continue game                                
        {
            mover.PlayAnimation(AnimationType.Idle);
            EventManager.GeneratePlatformEvent();
            EventManager.SetInputAvailableEvent(true);
            Debug.Log("Continue");
        }
        else                                                                    // level failed
        {
            mover.FallDown(jumpPower, jumpDuration, fallDistance);
            CanvasController.instance.SwitchCanvas(CanvasType.FailMenu);
            EventManager.LevelFailedEvent();
            Debug.Log("Level Failed");
        }
    } 
    #endregion

    #region Check Platform Below Player

    private PlatformObject CheckPlatform(float checkDis = 1f)
    {
        RaycastHit[] hits = Physics.RaycastAll(transform.position, -Vector3.up, checkDis);
        PlatformObject obj = null;

        foreach (RaycastHit hit in hits)
        {
            obj = hit.collider.GetComponent<PlatformObject>();
            if (obj) break;
        }

        return obj;
    }

    private FinishPlatform CheckFinishPlatform(float checkDis = 1f)
    {
        RaycastHit[] hits = Physics.RaycastAll(transform.position, -Vector3.up, checkDis);
        FinishPlatform obj = null;

        foreach (RaycastHit hit in hits)
        {
            obj = hit.collider.GetComponent<FinishPlatform>();
            if (obj) break;
        }

        return obj;
    }

    private FinishPlatform CheckFinishPlatformFront(float checkDis = 1f)
    {
        Vector3 originVec = transform.position + (Vector3.forward * zDiffValue);
        RaycastHit[] hitsFront = Physics.RaycastAll(originVec, -Vector3.up, checkDis);

        FinishPlatform obj = null;

        foreach (RaycastHit hit in hitsFront)
        {
            obj = hit.collider.GetComponent<FinishPlatform>();
            if (obj) break;
        }
        return obj;
    }
    #endregion

    #region Init Methods
    private void SetDiffZ()
    {
        GameObject clone = PoolManager.instance.platformPool.PullObjFromPool();
        zDiffValue = clone.GetComponent<MeshRenderer>().bounds.extents.z * 2f;
        PoolManager.instance.platformPool.AddObjToPool(clone);
    } 
    #endregion

    #region Disable Listeners
    private void OnDisable()
    {
        EventManager.MovePlayer -= MoveToPosition;
        EventManager.MovePlayerForward -= MoveForward;
        EventManager.CheckGameCondition -= CheckGame;
    } 
    #endregion
}
