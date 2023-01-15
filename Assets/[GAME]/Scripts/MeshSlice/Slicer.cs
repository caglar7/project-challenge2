using BzKovSoft.ObjectSlicer;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// slicing objects and getting slice negative and positive parts of mesh
/// </summary>

public class Slicer : MonoBehaviour
{
    [SerializeField] GameObject refObject;
    List<Transform> cutObjects = new List<Transform>();

    public void SetReferenceObject(GameObject g)
    {
        refObject = g;
    }
    
    /// <summary>
    /// get remaining platform part
    /// </summary>
    /// <param name="g"></param>
    /// <returns> scale of remaining platform for platform generator </returns>
    public Vector3 SliceObject(GameObject g)
    {
        Vector3 resultScale = g.transform.localScale;

        var sliceable = g.GetComponent<IBzSliceable>();
        if (sliceable == null) return resultScale;

        Bounds refBounds = refObject.GetComponent<MeshRenderer>().bounds;
        Bounds movingBounds = g.GetComponent<MeshRenderer>().bounds;
        float disLeft = -(refBounds.center.x - refBounds.extents.x);
        float disRight = (refBounds.center.x - refBounds.extents.x);

        // just 2 condition for now, later gonna have, perfect fit, no cut fail, small cut fail
        if (movingBounds.center.x < refBounds.center.x)
        {
            sliceable.Slice(new Plane(Vector3.right, disLeft), SetCutObjects);
            if (cutObjects.Count > 0)
            {
                cutObjects[0].GetComponent<Rigidbody>().isKinematic = false;
                SetReferenceObject(cutObjects[1].gameObject);
                resultScale = cutObjects[1].GetComponent<MeshRenderer>().bounds.extents * 2f;
            }
        }

        else if (movingBounds.center.x > refBounds.center.x)
        {
            sliceable.Slice(new Plane(Vector3.right, disRight), SetCutObjects);
            if (cutObjects.Count > 0)
            {
                cutObjects[1].GetComponent<Rigidbody>().isKinematic = false;
                SetReferenceObject(cutObjects[0].gameObject);
                resultScale = cutObjects[0].GetComponent<MeshRenderer>().bounds.extents * 2f;
            }
        }


        return resultScale;
    }

    private void SetCutObjects(BzSliceTryResult result)
    {
        cutObjects.Clear();
        if(result.outObjectNeg) cutObjects.Add(result.outObjectNeg.transform);
        if(result.outObjectPos) cutObjects.Add(result.outObjectPos.transform);
    }
}
