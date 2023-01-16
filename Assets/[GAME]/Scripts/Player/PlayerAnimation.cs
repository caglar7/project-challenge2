using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    [SerializeField] Animator animator;

    public void TriggerAnimation(AnimationType type)
    {
        animator.SetTrigger(type.ToString());
    }
}

public enum AnimationType
{
    Running,
    Idle,
    Fall,
}