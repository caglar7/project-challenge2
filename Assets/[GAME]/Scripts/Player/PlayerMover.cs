using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

/// <summary>
/// player mover component, controlled from PlayerControl.cs
/// </summary>

public class PlayerMover : MonoBehaviour
{
    [SerializeField] float speed = 5f;
    [SerializeField] float radiusCheck = .2f;
    bool isReached = true;
    Vector3 dir;
    Vector3 targetPosition;
    PlayerAnimation anim;

    private void Awake()
    {
        anim = GetComponent<PlayerAnimation>();
    }

    private void Update()
    {
        if(!isReached)
        {
            transform.position += (dir * speed * Time.deltaTime);

            if(Vector3.Distance(transform.position, targetPosition) <= radiusCheck)
            {
                isReached = true;
                EventManager.CheckGameConditionEvent();
            }
        }
    }

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

    public void FallDown(float jumpPower = 1f, float jumpDuration = 1f, float fallDis = 10f)
    {
        transform.DOJump(transform.position - (Vector3.up * fallDis), jumpPower, 1, jumpDuration)
            .OnComplete(() => {
                GetComponent<Rigidbody>().isKinematic = false; 
            });

        anim.TriggerAnimation(AnimationType.Fall);
    }

    public void PlayAnimation(AnimationType type)
    {
        anim.TriggerAnimation(type);
    }
}
