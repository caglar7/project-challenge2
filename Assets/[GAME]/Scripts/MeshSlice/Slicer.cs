using BzKovSoft.ObjectSlicer;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slicer : MonoBehaviour
{

    private GameObject refObject;

    public void SetReferenceObject(GameObject g)
    {
        refObject = g;
    }

    public void SliceObject(GameObject g)
    {
        var sliceable = g.GetComponent<IBzSliceable>();
        if (sliceable == null) return;

        sliceable.Slice(new Plane(Vector3.right, 0f), null);
    }
}
