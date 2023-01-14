using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] Transform initTarget;

    [Header("Fields")]
    PlayerMover mover;
    PlayerAnimation anim;

    private void Awake()
    {
        mover = GetComponent<PlayerMover>();
        anim = GetComponent<PlayerAnimation>();
    }

    private void Start()
    {
        MoveToPosition(initTarget.position);
        EventManager.MovePlayer += MoveToPosition;
    }

    private void MoveToPosition(Vector3 pos)
    {
        mover.Move(pos);
    }
}
