using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Sound
{
    public string clipName;
    public AudioClip clip;
    [Range(0f, 1f)] public float volume;
    [Range(1f, 2f)] public float pitch;
    [HideInInspector] public AudioSource audioSource;
}
