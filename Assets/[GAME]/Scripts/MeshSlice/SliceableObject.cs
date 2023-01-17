using BzKovSoft.ObjectSlicer;
using System;
using System.Diagnostics;
using UnityEngine;

/// <summary>
/// 
/// asset class
/// 
///  from Mesh Slicer asset, to make an object sliceable
///  we need to inherit from BZSliceableObjectBase
/// </summary>

public class SliceableObject : BzSliceableObjectBase
{
    #region Implemented

    protected override BzSliceTryData PrepareData(Plane plane)
    {
        // colliders that will be participating in slicing
        var colliders = gameObject.GetComponentsInChildren<Collider>();

        // return data
        return new BzSliceTryData()
        {
            // componentManager: this class will manage components on sliced objects
            componentManager = new StaticComponentManager(gameObject, plane, colliders),
            plane = plane,
        };
    }

    protected override void OnSliceFinished(BzSliceTryResult result)
    {

    } 

    #endregion
}
