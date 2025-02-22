using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SongManager : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip testSong;
    public AudioClip thiefSong;

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
}
