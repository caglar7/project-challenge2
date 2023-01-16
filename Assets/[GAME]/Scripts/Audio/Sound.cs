using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Sound
{
    public SoundType soundType;
    public AudioClip clip;
    [Range(0f, 1f)] public float volume;
    [Range(1f, 2f)] public float pitch;
    [HideInInspector] public AudioSource audioSource;
}
