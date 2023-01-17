using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    [Header("Initial Settings")]
    [SerializeField] Transform initTarget;

    //[Header("Win Settings")]

    [Header("Fail Settings")]
    [SerializeField] float jumpPower;
    [SerializeField] float jumpDuration;
    [SerializeField] float fallDistance;

    [Header("Fields")]
    PlayerMover mover;
    float zDiffValue;

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

    /// <summary>
    /// 3 conditions for game
    /// 
    /// continue
    /// win
    /// fail
    /// 
    /// </summary>
    private void CheckGame()
    {
        Debug.Log("check game condition");

        PlatformObject platformObject = CheckPlatform();
        FinishPlatform finishPlatform = CheckFinishPlatform();
        FinishPlatform finishPlatformFront = CheckFinishPlatformFront();

        if(platformObject && finishPlatformFront)                               // next block is finish
        {
            mover.Move(transform.position + new Vector3(0f, 0f, zDiffValue));
            Debug.Log("Finish is next");
        }
        else if(finishPlatform)                                                 // finish, level done
        {
            mover.PlayAnimation(AnimationType.Dance);
            CanvasController.instance.SwitchCanvas(CanvasType.WinMenu);
            EventManager.LevelWinEvent();
            Debug.Log("Level Win");
        }
        else if(platformObject)                                                 // continue game                                
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
    
    #region Level Condition Checks

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


    private void SetDiffZ()
    {
        GameObject clone = PoolManager.instance.platformPool.PullObjFromPool();
        zDiffValue = clone.GetComponent<MeshRenderer>().bounds.extents.z * 2f;
        PoolManager.instance.platformPool.AddObjToPool(clone);
    }

    private void OnDisable()
    {
        EventManager.MovePlayer -= MoveToPosition;
        EventManager.MovePlayerForward -= MoveForward;
        EventManager.CheckGameCondition -= CheckGame;
    }
}
