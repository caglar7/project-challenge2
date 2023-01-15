using BzKovSoft.ObjectSlicer;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// slicing objects and getting slice negative and positive parts of mesh
/// </summary>

public class Slicer : MonoBehaviour
{
    #region Properties
    [SerializeField] GameObject refObject;
    [SerializeField] float perfectThreshold = .2f;
    [SerializeField] float failThreshold = .1f;
    List<Transform> cutObjects = new List<Transform>();
    Vector3 resultScale; 
    #endregion

    #region Slice Method
    /// <summary>
    /// 
    /// slice conditions
    /// checking perfect fit, fail or slicing
    /// 
    /// </summary>
    /// <param name="movingObject"> the object that was moving and stopped to slice </param>
    /// <returns> scale of remaining platform for platform generator </returns>
    public Vector3 SliceObject(GameObject movingObject)
    {
        resultScale = movingObject.transform.localScale;

        var sliceable = movingObject.GetComponent<IBzSliceable>();
        if (sliceable == null) return resultScale;

        Bounds refBounds = refObject.GetComponent<MeshRenderer>().bounds;
        Bounds movingBounds = movingObject.GetComponent<MeshRenderer>().bounds;
        float disLeft = refBounds.center.x - refBounds.extents.x;
        float disRight = refBounds.center.x + refBounds.extents.x;
        float centerDiffX = Mathf.Abs(refBounds.center.x - movingBounds.center.x);
        float widthX = refBounds.extents.x * 2f;

        if (Mathf.Abs(movingBounds.center.x - refBounds.center.x) <= perfectThreshold)
        {
            PerfectFit(movingObject, refBounds, movingBounds);
        }
        else if (centerDiffX >= (widthX - failThreshold))
        {
            Fail(movingObject);
        }
        else
        {
            SliceMethod(sliceable, refBounds, movingBounds, disLeft, disRight);
        }

        return resultScale;
    } 
    #endregion

    #region Slice Condition Methods

    private void Fail(GameObject movingObject)
    {
        cutObjects.Clear();
        movingObject.GetComponent<Rigidbody>().isKinematic = false;
    }

    private void PerfectFit(GameObject movingObject, Bounds refBounds, Bounds movingBounds)
    {
        Vector3 nextMeshPos = refBounds.center;
        nextMeshPos.z = movingBounds.center.z;

        Vector3 meshMinusPivot = movingBounds.center - movingObject.transform.position;
        movingObject.transform.position = nextMeshPos - meshMinusPivot;

        refObject = movingObject;
    }

    private void SliceMethod(IBzSliceable sliceable, Bounds refBounds, Bounds movingBounds, float disLeft, float disRight)
    {
        if (movingBounds.center.x < refBounds.center.x)
        {
            sliceable.Slice(new Plane(Vector3.right, -disLeft), SetCutObjects);
            if (cutObjects.Count > 0)
            {
                cutObjects[0].GetComponent<Rigidbody>().isKinematic = false;    // later method, handle fallen obj
                refObject = cutObjects[1].gameObject;
                resultScale = cutObjects[1].GetComponent<MeshRenderer>().bounds.extents * 2f;
            }
        }

        else if (movingBounds.center.x > refBounds.center.x)
        {
            sliceable.Slice(new Plane(Vector3.right, -disRight), SetCutObjects);
            if (cutObjects.Count > 0)
            {
                cutObjects[1].GetComponent<Rigidbody>().isKinematic = false;
                refObject = cutObjects[0].gameObject;
                resultScale = cutObjects[0].GetComponent<MeshRenderer>().bounds.extents * 2f;
            }
        }
    }

    #endregion

    #region Get Sliced Objects

    /// <summary>
    /// slice method callback
    /// handling cutObjects list
    /// </summary>
    /// <param name="result"></param>
    private void SetCutObjects(BzSliceTryResult result)
    {
        cutObjects.Clear();
        if (result.outObjectNeg) cutObjects.Add(result.outObjectNeg.transform);
        if (result.outObjectPos) cutObjects.Add(result.outObjectPos.transform);
    }
    #endregion

    #region Get Position Data
    /// <summary>
    ///  returning value is used for passing next position to player to move
    /// </summary>
    /// <returns></returns>
    public Vector3 RemainingObjectPosition()
    {
        if (cutObjects.Count > 0) return refObject.GetComponent<MeshRenderer>().bounds.center;
        return Vector3.zero;
    } 
    #endregion
}
