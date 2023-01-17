using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    [SerializeField] Animator animator;

    private void Start()
    {
        Debug.Log("Player Animation start");
    }

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
    Dance,
}