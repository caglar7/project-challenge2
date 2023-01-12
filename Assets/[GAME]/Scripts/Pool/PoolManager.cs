using UnityEngine;

public class PoolManager : MonoBehaviour
{
    #region Singleton
    public static PoolManager Instance = null;

    private void Awake()
    {
        if (Instance == null) Instance = this;

        StartCreation();
    }
    #endregion

    [Header("Objects For Pooling")]
    public GameObject cube;

    [Header("Pools")]
    private PoolingPattern cubePool;

    void StartCreation()
    {

    }
}
