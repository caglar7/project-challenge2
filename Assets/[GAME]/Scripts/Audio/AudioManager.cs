using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// singleton class that controls one shot sounds
/// sound volume and pitch levels can be adjusted on inspector
/// </summary>

public class AudioManager : MonoBehaviour
{
    #region Properties
    [Header("Note Settngs")]
    [Range(0f, 1f)] public float noteVolume;
    [Range(.1f, 2f)] public float notePitch;

    public Sound[] sounds;
    public static AudioManager instance;
    private List<Sound> notes = new List<Sound>();
    private int currentNote;
    #endregion

    #region Awake, Singleton, init audio sources
    private void Awake()
    {
        if (instance == null) instance = this;
        else if (instance != this)
        {
            Destroy(gameObject);
            return;
        }

        foreach (Sound sound in sounds)
        {
            sound.audioSource = gameObject.AddComponent<AudioSource>();
            sound.audioSource.clip = sound.clip;
            sound.audioSource.volume = sound.volume;
            sound.audioSource.pitch = sound.pitch;

            if (sound.soundType == SoundType.Note)
            {
                sound.audioSource.volume = noteVolume;
                sound.audioSource.pitch = notePitch;
                notes.Add(sound);
            }
        }
    }
    #endregion

    #region Play Sound Methods
    public void PlayNextNote()
    {
        notes[currentNote].audioSource.Play();
        currentNote = Mathf.Clamp(++currentNote, 0, notes.Count - 1);
    }

    public void ResetNote()
    {
        currentNote = 0;
    }

    public void SliceSound()
    {
        Sound s = Array.Find(sounds, s => s.soundType == SoundType.Sliced);
        s.audioSource.pitch = UnityEngine.Random.Range(s.pitch - .1f, s.pitch + .1f);
        s.audioSource.Play();
    } 
    #endregion
}

public enum SoundType
{
    Note,
    Sliced,
}
