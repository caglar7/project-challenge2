using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// animation component
/// </summary>

public class PlayerAnimation : MonoBehaviour
{
    #region Properties
    [SerializeField] Animator animator;
    #endregion

    #region Methods
    public void TriggerAnimation(AnimationType type)
    {
        animator.SetTrigger(type.ToString());
    } 
    #endregion
}

public enum AnimationType
{
    Running,
    Idle,
    Fall,
    Dance,
}