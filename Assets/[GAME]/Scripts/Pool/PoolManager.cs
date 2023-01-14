using UnityEngine;

public class PoolManager : MonoBehaviour
{
    #region Singleton
    public static PoolManager instance = null;

    private void Awake()
    {
        if (instance == null) instance = this;

        StartCreation();
    }
    #endregion

    [Header("Objects For Pooling")]
    public GameObject platform;

    [Header("Pools")]
    [HideInInspector] public PoolingPattern platformPool;

    void StartCreation()
    {
        platformPool = new PoolingPattern(platform);
        platformPool.FillPool(25);
    }
}
