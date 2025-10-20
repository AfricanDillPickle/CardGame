using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using System;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;
    public static AudioManager instance;
    void Awake()
    {
        //DontDestroyOnLoad(gameObject);

        if (instance == null)
        {
            instance = this;
        } else
        {
            Destroy(gameObject);
            return;
        }

        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
    }



    public void Play (string name, float pitch = 0)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (pitch != 0)
        {
            s.source.pitch = pitch;
        }
        if (s == null)
        {
            return;
        }
        s.source.Play();
    }

    public void StopAll()
    {
        foreach (Sound s in sounds)
        {
            Sound currentSound = Array.Find(sounds, item => item.name == s.name);

            currentSound.source.Stop();

        }
    }

    public void StopPlaying(string sound)
    {
        Sound s = Array.Find(sounds, item => item.name == sound);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }

        s.source.Stop();
    }

}
