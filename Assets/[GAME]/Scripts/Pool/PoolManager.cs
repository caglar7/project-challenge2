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
    public GameObject money;
    public GameObject ball;
    public GameObject effectRemove;
    public GameObject scorePoint;

    [Header("Pools")]
    [HideInInspector] public PoolingPattern moneyPool;
    [HideInInspector] public PoolingPattern ballPool;
    [HideInInspector] public PoolingPattern effectRemovePool;
    [HideInInspector] public PoolingPattern scorePointPool;

    void StartCreation()
    {
        ballPool = new PoolingPattern(ball);
        ballPool.FillPool(100);

        effectRemovePool = new PoolingPattern(effectRemove);
        effectRemovePool.FillPool(50);

        scorePointPool = new PoolingPattern(scorePoint);
        scorePointPool.FillPool(50);
    }
}
