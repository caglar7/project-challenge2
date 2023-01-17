using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

/// <summary>
/// player mover component, controlled from PlayerControl.cs
/// </summary>

public class PlayerMover : MonoBehaviour
{
    #region Properties

    [SerializeField] float speed;
    [SerializeField] float radiusCheck;
    bool isReached = true;
    Vector3 dir, targetPosition;
    PlayerAnimation anim;
    #endregion

    #region Awake, Update
    private void Awake()
    {
        anim = GetComponent<PlayerAnimation>();
    }

    private void Update()
    {
        if (!isReached)
        {
            transform.position += (dir * speed * Time.deltaTime);

            if (Vector3.Distance(transform.position, targetPosition) <= radiusCheck)
            {
                isReached = true;
                EventManager.CheckGameConditionEvent();
            }
        }
    }
    #endregion

    #region Movement Related
    /// <summary>
    /// move to position, called from PlayerControl.cs
    /// </summary>
    /// <param name="targetPos"></param>
    public void Move(Vector3 targetPos)
    {
        targetPos.y = transform.position.y;
        targetPosition = targetPos;
        Vector3 dirDiff = (targetPos - transform.position);
        dir = dirDiff.normalized;
        isReached = false;
        RotateTowards(dir, .5f);

        PlayAnimation(AnimationType.Running);
    }

    private void RotateTowards(Vector3 dir, float duration)
    {
        transform.DORotateQuaternion(Quaternion.LookRotation(dir), duration);
    }
    #endregion

    #region Other Methods

    public void FallDown(float jumpPower = 1f, float jumpDuration = 1f, float fallDis = 10f)
    {
        transform.DOJump(transform.position - (Vector3.up * fallDis), jumpPower, 1, jumpDuration)
            .OnComplete(() =>
            {
                GetComponent<Rigidbody>().isKinematic = false;
            });

        anim.TriggerAnimation(AnimationType.Fall);
    }

    public void PlayAnimation(AnimationType type)
    {
        anim.TriggerAnimation(type);
    } 
    #endregion
}
