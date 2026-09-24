using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    private bool muted = false;

    public Sound[] sounds;

    public static AudioManager Instance;

    void Awake()
    {
        PlayerPrefs.SetInt("Sound", 1);
        if (Instance == null) {
            Instance = this;
        }else {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

        foreach (Sound s in sounds) {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
    }

    void Start()
    {
        Play("Theme");
    }

    void Update()
    {
        if (PlayerPrefs.GetInt("Sound", 0) == 0) {
            foreach (Sound s in sounds) {
                s.source.volume = 0;
            }
            muted = true;
        }else if (PlayerPrefs.GetInt("Sound", 0) == 1) {
            foreach (Sound s in sounds) {
                s.source.volume = 1;
            }
            muted = false;
        }
    }

    public void Play (string name) {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        s.source.Play();
    }
}
