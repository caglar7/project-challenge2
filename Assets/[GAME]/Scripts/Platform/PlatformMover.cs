using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// platform movement component
/// </summary>

public class PlatformMover : MonoBehaviour
{
    #region Properties
    [SerializeField] float maxSpeed;
    float speed, boundPositive, boundNegative;
    bool isMoving = false, canSetDir = true;
    Vector3 dir;
    #endregion

    #region Update
    private void Update()
    {
        if (isMoving)
        {
            transform.position += (dir * speed * Time.deltaTime);
            if (transform.position.x >= boundPositive || transform.position.x <= boundNegative)
            {
                if (canSetDir)
                {
                    canSetDir = false;
                    dir.x = -dir.x;
                    StartCoroutine(SetDirOnTimeCo());
                }
            }
        }
    }

    IEnumerator SetDirOnTimeCo()
    {
        yield return new WaitForSeconds(.2f);
        canSetDir = true;
    }
    #endregion

    #region Methods
    /// <summary>
    /// init diri movement settings
    /// <param name="startPos"> start at left and right </param>
    /// <param name="startDir"></param>
    /// <param name="speedMult"> fraction of max speed, 1f for max speed </param>
    public void StartMoving(Vector3 startPos, float speedMult)
    {
        speed = Mathf.Clamp01(speedMult) * maxSpeed;
        transform.position = startPos;
        dir = (startPos.x < 0f) ? -Vector3.right : Vector3.right;
        boundPositive = Mathf.Abs(startPos.x);
        boundNegative = -Mathf.Abs(startPos.x);
        isMoving = true;
    }

    public void StopMoving()
    {
        isMoving = false;
    } 
    #endregion
}
