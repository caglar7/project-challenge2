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

    private void Awake()
    {
        mover = GetComponent<PlayerMover>();
    }

    private void Start()
    {
        MoveToPosition(initTarget.position);
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
        PlatformObject platformObject = null;
        FinishPlatform finishPlatform = null;
        RaycastHit[] hits = CheckGround(1f);
        foreach(RaycastHit hit in hits)
        {
            if (platformObject == null) platformObject = hit.collider.GetComponent<PlatformObject>();
            if (finishPlatform == null) finishPlatform = hit.collider.GetComponent<FinishPlatform>();
        }

        if(finishPlatform)
        {
            EventManager.LevelWinEvent();
            Debug.Log("Level Win");
        }
        else if(platformObject)
        {
            EventManager.GeneratePlatformEvent();   // temp
            EventManager.SetInputAvailableEvent(true);
        }
        else
        {
            mover.FallDown(jumpPower, jumpDuration, fallDistance);
            EventManager.LevelFailedEvent();
            Debug.Log("Level Failed");
        }
    }

    private RaycastHit[] CheckGround(float checkDis = 1f)
    {
        return Physics.RaycastAll(transform.position, -Vector3.up, checkDis);
    }

}
