using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SongManager : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip testSong;
    public AudioClip thiefSong;
    public AudioClip wing1Song;
    public AudioClip aldricSong;

    //songs
    public void playTestSong()
    {
        audioSource.clip = testSong;
        audioSource.Play(); 
    }

    public void playThiefSong()
    {
        audioSource.clip = thiefSong;
        audioSource.Play(); 
    }

    public void playWing1Song()
    {
        audioSource.clip = wing1Song;
        audioSource.Play();
    }

    public void playAldricSong()
    {
        audioSource.clip = aldricSong;
        audioSource.Play();
    }

    //stop song
    public void stopSong()
    {
        audioSource.Stop();
    }
}
