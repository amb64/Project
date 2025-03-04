using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource buttonSound;
    public AudioSource bgMusic;
    public AudioSource bgMusicDistorted;
    public AudioSource mouse;
    public AudioSource heartbeat;

    public AudioSource notif;

    public void StartAudio()
    {
        // Start playing and then stop the music so that we can unpause it later depending on stats
        bgMusic.loop = true;
        bgMusicDistorted.loop = true;
        
        bgMusic.Play();

        bgMusicDistorted.Play();
        bgMusicDistorted.Pause();

        heartbeat.Play();
        heartbeat.Pause();
    }

    public void Button()
    {
        buttonSound.Play();
    }

    public void Notif()
    {
        notif.Play();
    }

    public void Mouse()
    {
        mouse.Play();
    }

    public void Heartbeat()
    {
        heartbeat.UnPause();
        //Debug.Log("play heartbeat sound");
    }

    public void StopHeartbeat()
    {
        heartbeat.Pause();
    }

    public void Distort()
    {
        bgMusic.Pause();
        bgMusicDistorted.UnPause();
    }

    public void UnDistort()
    {
        bgMusic.UnPause();
        bgMusicDistorted.Pause();
    }
}
