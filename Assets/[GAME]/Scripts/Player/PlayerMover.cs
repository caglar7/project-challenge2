using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    private void Update()
    {
        if(!isReached)
        {
            transform.position += (dir * speed * Time.deltaTime);
            if(Vector3.Distance(transform.position, targetPosition) <= radiusCheck)
            {
                isReached = true;

                // trigger events
                
                EventManager.GeneratePlatformEvent();   // temp
                EventManager.SetInputAvailableEvent(true);
            }
        }
    }

    public void Move(Vector3 targetPos)
    {
        targetPos.y = transform.position.y;
        targetPosition = targetPos;
        Vector3 dirDiff = (targetPos - transform.position);
        dir = dirDiff.normalized;
        isReached = false;
    }
}
