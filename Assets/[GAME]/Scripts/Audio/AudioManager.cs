using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;
    public static AudioManager instance;

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
        }
    }

    public void PlayNote(float pitchRiseRatio = 0f)
    {
        Sound sound = Array.Find(sounds, s => s.clipName == SoundName.Note.ToString());
        sound.audioSource.pitch += (sound.pitch * pitchRiseRatio);
        sound.audioSource.Play();
    }
    
    public void ResetNote()
    {
        Sound sound = Array.Find(sounds, s => s.clipName == SoundName.Note.ToString());
        sound.audioSource.pitch = sound.pitch;
    }
}

public enum SoundName
{
    Note,
    Sliced,
}
