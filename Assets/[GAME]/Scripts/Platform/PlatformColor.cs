using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///  setting colors with material property block
///  this class is also called from sliced objects to assign
///  colors to the sliced meshes
/// </summary>

public class PlatformColor : MonoBehaviour
{
    #region Properties
    MaterialPropertyBlock matPB;
    [SerializeField] Color[] colors;
    [SerializeField] bool setInitColor;
    #endregion

    #region Start
    private void Start()
    {
        if (setInitColor)
        {
            SetColor(colors.Length - 1);
        }
    }
    #endregion

    #region Set Color Method

    public void SetColor(int index)
    {
        matPB = new MaterialPropertyBlock();
        MeshRenderer rend = GetComponent<MeshRenderer>();

        rend.GetPropertyBlock(matPB);
        index %= colors.Length;
        matPB.SetColor("_Color", colors[index]);
        rend.SetPropertyBlock(matPB);
    } 
    #endregion
}
