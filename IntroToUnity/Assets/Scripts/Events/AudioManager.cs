using UnityEngine.Audio;
using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("Sounds")]
    public Audio[] audios;

    private void Awake()
    {
        foreach(Audio s in audios)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.audio;

            s.source.volume = s.volume;
            s.source.loop = s.loop;
        }
    }

    public void Play(string name)
    {
        Audio s = Array.Find(audios, audio => audio.name == name);
        s.source.Play();
    }
}
