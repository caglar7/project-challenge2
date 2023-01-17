using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

/// <summary>
///  start rotation when level win event trigger
/// </summary>

public class CameraControl : MonoBehaviour
{
    [SerializeField] float rotationSpeed = 100f;
    [SerializeField] float lerpMult = 10f;
    bool isRotating = false;
    CinemachineVirtualCamera myCamera;

    private void Awake()
    {
        myCamera = GetComponent<CinemachineVirtualCamera>();
        EventManager.LevelWin += StartRotating;
    }

    private void Update()
    {
        if(isRotating)
        {
            Vector3 rot = transform.eulerAngles;
            rot += new Vector3(0f, rotationSpeed * Time.deltaTime, 0f);
            transform.eulerAngles = Vector3.Lerp(transform.eulerAngles, rot, Time.deltaTime * lerpMult);
        }
    }

    private void StartRotating()
    {
        ResetOffset();
        isRotating = true;
    }

    private void StopRotating()
    {
        isRotating = false;
    }

    private void ResetOffset()
    {
        var transposer = myCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
        transposer.m_TrackedObjectOffset = Vector3.zero;
    }

    private void OnDisable()
    {
        EventManager.LevelWin -= StartRotating;
    }
}
