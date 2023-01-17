using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("Note Settngs")]
    [Range(0f, 1f)] public float noteVolume = 1f;
    [Range(.1f, 2f)] public float notePitch = 1f;

    public Sound[] sounds;
    public static AudioManager instance;
    private List<Sound> notes = new List<Sound>();
    private int currentNote = 0;

    private void Awake()
    {
        if (instance == null) instance = this;
        else if(instance != this)
        {
            Destroy(gameObject);
            return;
        }

        foreach(Sound sound in sounds)
        {
            sound.audioSource = gameObject.AddComponent<AudioSource>();
            sound.audioSource.clip = sound.clip;
            sound.audioSource.volume = sound.volume;
            sound.audioSource.pitch = sound.pitch;
            
            if(sound.soundType == SoundType.Note)
            {
                sound.audioSource.volume = noteVolume;
                sound.audioSource.pitch = notePitch;
                notes.Add(sound);
            }
        }
    }

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
}

public enum SoundType
{
    Note,
    Sliced,
}
