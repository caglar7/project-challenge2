using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// access UI element
/// </summary>

public class LevelCount : MonoBehaviour
{
    public static LevelCount instance;
    private void Awake()
    {
        if (instance == null) instance = this;
    }

    public TextMeshProUGUI tmpro;
}
