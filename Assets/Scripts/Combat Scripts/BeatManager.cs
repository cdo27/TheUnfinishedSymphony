using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Collections;

public class BeatManager : MonoBehaviour
{
    //---------------------------initiate all the necessary objects and components------------------------------------
    //current song that will be played, when loading combat this should be loaded based on which level
    public Song currentSong;

    //managers
    public SongManager songManager;
    public AudioManager audioManager;
    public CombatStateManager combatStateManager;
    public NoteSpawner noteSpawner;

    //variables carrying current attribute of songs
    public bool songStarted = false;
    private float songBPM; //bpm, carry over from currentSong
    private float crotchet; // Duration of one beat
    public double dspTimeSongStart; //the reference time of when the song actually starts
    public double dspTimeOffset; // original difference between song start dsp time and actual dsp time at the time.

    //for identifying individual beats
    private double nextBeatTime; //the next beat time, when dsp time reaches this time, play a beat.

    public int currentBeat;  //calculate current beat

    //current list of active notes
    private List<Note> activeNotes = new List<Note>();

    //magic shield object
    public GameObject musicShield;

    void Update()
    {
        //press space bar to start the song
        if (!songStarted && Input.GetKeyDown(KeyCode.Space))
        {
            startSong();
        }

        //----------------------------------code for gameplay once song started-------------------------------------
        if (songStarted)
        {
            CheckAndSpawnNotes(); // Check and spawn notes based on beats before they arrive
            CheckAndDeleteNotes(); // check if there's any destroyed notes, if yes remove from list

            //player hit
            if (Input.GetKeyDown(KeyCode.A))
            {
                if (combatStateManager.gameState == 2)
                {
                    musicShield.SetActive(true);
                    Invoke("HideMusicShield", 0.1f); // Hides after 0.1 seconds
                }

                if(combatStateManager.gameState == 1) audioManager.playHitSoundA();

                bool noteHit = false; // Flag to track if we've already hit a note.

                // Loop through all active notes and check if one can be hit
                for (int i = activeNotes.Count - 1; i >= 0; i--)
                {
                    Note note = activeNotes[i];

                    // Call checkIfHit() for each note to determine if it's hit
                    int hitResult = note.checkIfHit();

                    // Check if the hit result is either perfect (2) or slightly missed (1), and hit only one note at a time
                    if (!noteHit && (hitResult == 2 || hitResult == 1))
                    {
                        noteHit = true; // Mark that we've hit a note

                        //play block sound if blocking an attack 
                        if (combatStateManager.gameState == 2) audioManager.playMusicBlockSound();

                        // If it's a perfect hit
                        if (hitResult == 2)
                        {
                            Debug.Log("Perfect hit!");
                        }
                        // If it's a slight miss
                        else if (hitResult == 1)
                        {
                            Debug.Log("Slight miss!");
                        }

                        // Destroy the note after hitting it
                        Destroy(note.gameObject);

                        // Break the loop to ensure only one note is hit per press
                        break;
                    }
                }
            }

            if (AudioSettings.dspTime >= nextBeatTime) // Schedule the next beat only when the current DSP time is beyond the next beat
            {
                ScheduleNextBeat();
            }
        }

    }

    //game initialization. Set up the song before playing
    void startSong()
    {
        songStarted = true;
        currentSong = new TestSong(); //insert whatever the current level song is
        songBPM = currentSong.BPM; //carry over BPM
        currentBeat = 0;

        crotchet = 60.0f / songBPM; // calculate uration of a single beat
        noteSpawner.setNoteSpeed(); // update note speed based on BPM

        currentSong.PlaySong(songManager); //play the song
        dspTimeSongStart = AudioSettings.dspTime; // save reference time for future calculations

        nextBeatTime = dspTimeSongStart + currentSong.offset; //next beat sound time
    }

    //---------------------------playing beat sounds every whole beat-----------------------------
    void ScheduleNextBeat()
    {
        //audioManager.playBeatSound(nextBeatTime);
        //if (currentBeat == 8) currentBeat = 0;
        currentBeat++;

        //end of enemy notes spawning during defend, play this quick sound
        if (currentBeat % 8 == 0 && combatStateManager.gameState == 2)
        {
            audioManager.playEndEnemyNoteSpawnSound();
        }
        Debug.Log(currentBeat);

        nextBeatTime += crotchet;
    }

    //------------------------------checking if notes need to be spawned, and spawn them-----------------------------
    int processingDefendList = 0; // Turn to 1 when processing a defend list
    int currentDefendListSize = 0;

    void CheckAndSpawnNotes()
    {
        if (currentSong.attackBeatsToHit.Count > 0 || currentSong.defendBeatsToHit.Count > 0)
        {
            float nextBeat;
            double dspTimeForNoteSpawn;

            switch (combatStateManager.gameState)
            {
                case 1:  // Attack Mode
                    if (currentSong.attackBeatsToHit.Count > 0)
                    {
                        nextBeat = currentSong.attackBeatsToHit[0]; // Get the first beat for attack mode

                        dspTimeForNoteSpawn = GetDspTimeForBeat(nextBeat - 4);  // Calculate spawn time, 4 beats before the current beat

                        if (AudioSettings.dspTime >= dspTimeForNoteSpawn && AudioSettings.dspTime < dspTimeForNoteSpawn + crotchet)
                        {
                            Note createdNote = noteSpawner.SpawnNote(nextBeat);
                            activeNotes.Add(createdNote);

                            currentSong.attackBeatsToHit.RemoveAt(0); // Remove the beat from the list after it has been processed
                        }
                    }
                    break;

                case 2:  // Defend Mode
                    if (currentSong.defendBeatsToHit.Count > 0)
                    {
                        // If we are not already processing a list, store its size
                        if (processingDefendList == 0)
                        {
                            processingDefendList = 1;  // Mark as processing
                            currentDefendListSize = currentSong.defendBeatsToHit[0].Count;  // Store list size
                        }

                        nextBeat = currentSong.defendBeatsToHit[0][0]; // Get the first beat for defend mode

                        dspTimeForNoteSpawn = GetDspTimeForBeat(nextBeat - 8);  // Calculate spawn time, 8 beats before the current beat

                        if (AudioSettings.dspTime >= dspTimeForNoteSpawn && AudioSettings.dspTime < dspTimeForNoteSpawn + crotchet)
                        {
                            int position = currentDefendListSize - currentSong.defendBeatsToHit[0].Count + 1; // Get note position

                            Note createdNote = noteSpawner.SpawnDefendNote(nextBeat, currentDefendListSize, position);
                            audioManager.playEnemyNotePopSound();
                            activeNotes.Add(createdNote);

                            currentSong.defendBeatsToHit[0].RemoveAt(0);

                            // If the list is now empty, reset variables and remove it
                            if (currentSong.defendBeatsToHit[0].Count == 0)
                            {
                                currentSong.defendBeatsToHit.RemoveAt(0);
                                processingDefendList = 0; // Reset processing flag
                                currentDefendListSize = 0; // Reset list size
                            }
                        }
                    }
                    break;
            }
        }
    }

    //------------------------------checking if notes need to be deleted, and delete them-----------------------------
    void CheckAndDeleteNotes()
    {
        // Iterate through the activeNotes list from the last to the first element
        for (int i = activeNotes.Count - 1; i >= 0; i--)
        {
            Note note = activeNotes[i];

            // Check if the note is null (which means it's been destroyed) or if it's no longer in the scene
            if (note == null)
            {
                // Remove the destroyed note from the list
                activeNotes.RemoveAt(i);
            }
        }
    }

    //------------------------------for conversion from beattime to dsptime-----------------------------
    public double GetDspTimeForBeat(float beatTime)
    {
        // Convert the beat time to DSP time relative to the song's start
        return dspTimeSongStart + (beatTime * crotchet) + currentSong.offset;
    }

    //-------------------------retrieve methods---------------------------------
    public float getCrotchet()
    {
        return crotchet;
    }

    public int getCurrentBeat()
    {
        return currentBeat;
    }

    void HideMusicShield()
    {
        musicShield.SetActive(false);
    }

}