using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

/// <summary>
///  cinemachine camera control, rotation etc.
/// </summary>

public class CameraControl : MonoBehaviour
{
    #region Properties
    [SerializeField] float rotationSpeed;
    [SerializeField] float lerpMult;
    bool isRotating = false;
    CinemachineVirtualCamera myCamera;
    #endregion

    #region Awake, Update

    private void Awake()
    {
        myCamera = GetComponent<CinemachineVirtualCamera>();
        EventManager.LevelWin += StartRotating;
    }

    private void Update()
    {
        if (isRotating)
        {
            Vector3 rot = transform.eulerAngles;
            rot += new Vector3(0f, rotationSpeed * Time.deltaTime, 0f);
            transform.eulerAngles = Vector3.Lerp(transform.eulerAngles, rot, Time.deltaTime * lerpMult);
        }
    }
    #endregion

    #region Camera Methods, Rotation, Offset Reset
    private void StartRotating()
    {
        ResetOffset();
        isRotating = true;
    }

    private void ResetOffset()
    {
        var transposer = myCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
        transposer.m_TrackedObjectOffset = Vector3.zero;
    }
    #endregion

    #region Remove Listeners on disable

    private void OnDisable()
    {
        EventManager.LevelWin -= StartRotating;
    } 
    #endregion
}
