using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource audioSource; 
    public AudioSource quillAudioSource; // Separate AudioSource for quill SFX
    public AudioClip beat;
    public AudioClip hitSoundA;
    public AudioClip enemyNotePop;
    public AudioClip musicBlock;
    public AudioClip endEnemyNoteSpawn;

    //BGM & SFX
    public AudioClip introBGM;
    public AudioClip quillSFX; 

    public void playBeatSound(double playTime)
    {
        audioSource.clip = beat;
        audioSource.PlayScheduled(playTime);
    }

    public void playHitSoundA()
    {
        audioSource.clip = hitSoundA;
        audioSource.PlayOneShot(hitSoundA);
    }

    public void playEnemyNotePopSound()
    {
        audioSource.clip = enemyNotePop;
        audioSource.PlayOneShot(enemyNotePop);
    }

    public void playMusicBlockSound()
    {
        audioSource.clip = musicBlock;
        audioSource.PlayOneShot(musicBlock);
    }

    public void playEndEnemyNoteSpawnSound()
    {
        audioSource.clip = endEnemyNoteSpawn;
        audioSource.PlayOneShot(endEnemyNoteSpawn);
        Debug.Log("enemy note stop spawning!");
    }



//BACKGROUND MUSIC AND SFX
    public void playIntroMusic() 
    {
        if (audioSource != null && introBGM != null)
        {
            audioSource.clip = introBGM;
            audioSource.loop = true;
            audioSource.Play();
        }
        else
        {
            Debug.LogError("AudioSource or introBGM not assigned!");
        }
    }

    public void StopIntroMusic()
    {
        if (audioSource != null)
        {
            audioSource.Stop();
        }
    }

    public void PlayQuillSFX()
    {
        if (quillAudioSource != null && quillSFX != null)
        {
            quillAudioSource.clip = quillSFX;
            quillAudioSource.loop = true; // 🔹 Ensure it loops while typing
            quillAudioSource.Play();
        }
    }

    public void StopQuillSFX()
    {
        if (quillAudioSource != null)
        {
            quillAudioSource.Stop();
        }
    }
    
}
