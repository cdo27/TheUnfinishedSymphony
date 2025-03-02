using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource audioSource; 
    public AudioSource bgmAudioSource; // Separate AudioSource for background music
    public AudioSource quillAudioSource; // Separate AudioSource for quill SFX
    public AudioClip beat;
    public AudioClip hitSoundA;
    public AudioClip enemyNotePop;
    public AudioClip musicBlock;
    public AudioClip endEnemyNoteSpawn;
    public AudioClip coinCollectSound;
    public AudioClip sheetCollectSound;
   
    //BGM & SFX
    public AudioClip introBGM;
    public AudioClip quillSFX; 
    public AudioClip backgroundMusic; // Background music clip
    private bool hasPlayedIntroBGM = false;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

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
        if (bgmAudioSource != null && introBGM != null && !hasPlayedIntroBGM)
    {
            bgmAudioSource.clip = introBGM;
            bgmAudioSource.loop = false;
            bgmAudioSource.Play();
            hasPlayedIntroBGM = true;

            // Start background music after introBGM finishes
            StartCoroutine(WaitForIntroToEnd());
        }
        else
        {
            Debug.LogError("AudioSource or introBGM not assigned!");
        }
    }

    public void StopIntroMusic()
    {
        if (bgmAudioSource != null)
        {
            bgmAudioSource.Stop();
        }
    }

    private IEnumerator WaitForIntroToEnd()
    {
         while (bgmAudioSource.isPlaying) // Wait until introBGM finishes
        {
            yield return null;
        }
        PlayBackgroundMusic();
    }

    public void PlayBackgroundMusic()
    {
        if (bgmAudioSource != null && backgroundMusic != null)
        {
            if (!bgmAudioSource.isPlaying) // Prevents overlapping music
            {
                bgmAudioSource.clip = backgroundMusic;
                bgmAudioSource.loop = true;
                bgmAudioSource.volume = 0.1f;
                bgmAudioSource.Play();
            }
        }
    }

    public void StopBackgroundMusic()
    {
        if (bgmAudioSource != null)
        {
            bgmAudioSource.Stop();
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
    // Coin & Sheet
    public void PlayCoinCollectSound()
    {
        audioSource.PlayOneShot(coinCollectSound);
    }
    public void PlaySheetCollectSound()
{
    audioSource.PlayOneShot(sheetCollectSound);
}
    
}
