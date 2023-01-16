using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// main canvas and switching methods
/// 
/// when any more new subcanvas added
/// update the enum below
/// </summary>

public enum CanvasType
{
    FailMenu,
    WinMenu,
}

public class CanvasController : MonoBehaviour
{ 
    SubCanvas[] subCanvases;
    public static CanvasController instance;
    private void Awake()
    {
        if (instance == null) instance = this;
        else if (instance != this) Destroy(gameObject);

        subCanvases = GetComponentsInChildren<SubCanvas>(true);
    }

    /// <summary>
    /// deactive canvases and activate the next one
    /// </summary>
    /// <param name="startCanvas"></param>
    public void SwitchCanvas(CanvasType startCanvas)
    {
        foreach(SubCanvas sub in subCanvases)
        {
            if (sub.canvasType == startCanvas) sub.gameObject.SetActive(true);
            else sub.gameObject.SetActive(false);
        }
    }
}
