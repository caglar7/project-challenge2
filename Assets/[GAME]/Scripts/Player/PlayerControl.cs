using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] Transform initTarget;

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
    }

    private void MoveToPosition(Vector3 pos)
    {
        mover.Move(pos);
    }
}