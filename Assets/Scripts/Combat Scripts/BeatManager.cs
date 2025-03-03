using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Collections;
using Unity.VisualScripting;

public class BeatManager : MonoBehaviour
{
    //---------------------------initiate all the necessary objects and components------------------------------------
    //managers
    public SongManager songManager;
    public AudioManager audioManager;
    public CombatStateManager combatStateManager;
    public AdvantageBarManager advantageBarManager;
    public NoteSpawner noteSpawner;

    //animator
    public Animator lucienAnimator;

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

    //not hit area lighted up object
    public GameObject highLightedHitArea;
    //magic shield object
    public GameObject redShield;
    public GameObject greenShield;
    public GameObject purpleShield;

    //note hit messages
    public GameObject perfectHitMessage;
    public GameObject nearMissMessage;
    public GameObject perfectBlockMessage;
    public GameObject nearMissBlockMessage;

    //tutorial messages
    public GameObject tutorialAMessage;
    public GameObject tutorialBMessage;
    public GameObject tutorialCMessage;
    public GameObject tutorialDMessage;
    public GameObject tutorialEMessage;
    public GameObject tutorialFMessage;
    public GameObject tutorialGMessage;
    public GameObject tutorialHMessage;

    private void Start()
    {
        // Get AudioManager reference
        audioManager = FindObjectOfType<AudioManager>();
        if (audioManager == null)
        {
            Debug.LogError("AudioManager not found in the scene!");
        }

        // Get AudioManager reference
        songManager = FindObjectOfType<SongManager>();
        if (songManager == null)
        {
            Debug.LogError("AudioManager not found in the scene!");
        }
 
    }

    void Update()
    { 
        //-----------------------------------check for win or lose condition---------------------------------------
        if(songStarted && AudioSettings.dspTime >=  GetDspTimeForBeat(combatStateManager.currentSong.songcompleteBeat))
        {
            if(advantageBarManager.CheckVictoryCondition() == true)
            {
                combatStateManager.gameState = 98; 
            }
            else if (advantageBarManager.CheckVictoryCondition() == false)
            {
                combatStateManager.gameState = 99;
            }

        }

        //----------------------------------code for gameplay once song started-------------------------------------
        if (songStarted && combatStateManager.gameState != 98 && combatStateManager.gameState != 99)
        {
            CheckAndSpawnNotes(); // Check and spawn notes based on beats before they arrive
            CheckAndDeleteNotes(); // check if there's any destroyed notes, if yes remove from list

            // Player hit
            if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow) ||
                Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow) ||
                Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
            {
                // Set up default behavior when the player presses the key during attack and defend mode
                if (combatStateManager.gameState == 1) audioManager.playHitSoundA();
                if (combatStateManager.gameState == 2)
                {
                    //play defend animation
                    lucienAnimator.SetBool("isDefending", true);
                    StartCoroutine(ResetDefendAnimation());
                    // generate appropriate shields
                    if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
                    {
                        ActivateShield(redShield);
                    }
                    else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
                    {
                        ActivateShield(greenShield);
                    }
                    else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
                    {
                        ActivateShield(purpleShield);
                    }
                }

                bool noteHit = false; // Flag to track if we've already hit a note.

                // Loop through all active notes and check if one can be hit
                for (int i = activeNotes.Count - 1; i >= 0; i--)
                {
                    Note note = activeNotes[i];

                    // Determine if the player pressed the correct key for this note type
                    bool correctKeyPressed = false;

                    if ((note.noteType == 0 && (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))) ||  // Red notes
                        (note.noteType == 1 && (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))) ||  // Green notes
                        (note.noteType == 2 && (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))))   // Purple notes
                    {
                        correctKeyPressed = true;
                    }

                    if (!correctKeyPressed) continue; // Skip this note if the wrong key was pressed

                    // Call checkIfHit() for each note to determine if it's hit
                    int hitResult = note.checkIfHit();

                    // Check if the hit result is either perfect (2) or slightly missed (1), and hit only one note at a time
                    if (!noteHit && (hitResult == 2 || hitResult == 1))
                    {
                        noteHit = true; // Mark that we've hit a note
                        highLightedHitArea.SetActive(true);
                        Invoke("HideHighlightedHitArea", 0.05f);

                        // Play block sound if blocking an attack
                        if (combatStateManager.gameState == 2)
                        {
                            audioManager.playMusicBlockSound();            
                        }

                        // handle hit and destroy, depending on if it's an atatck note or defend note
                        note.handleHit(combatStateManager.enemyHitPoint.transform.position);

                        // If it's a perfect hit
                        if (hitResult == 2)
                        {
                            if (combatStateManager.gameState == 1)
                            {
                                perfectHitMessage.SetActive(true);
                                advantageBarManager.HandleAttack("Perfect");
                                Invoke("HidePerfectHitMessage", 0.2f); // Hides after 0.2 seconds
                                //play attack animation
                                lucienAnimator.SetBool("isAttacking", true);
                                StartCoroutine(ResetAttackAnimation());
                            } else if (combatStateManager.gameState == 2)
                            {
                                perfectBlockMessage.SetActive(true); 
                                Invoke("HidePerfectBlockMessage", 0.2f); // Hides after 0.2 seconds    
                            }
                        }
                        // If it's a slight miss
                        else if (hitResult == 1)
                        {
                            if (combatStateManager.gameState == 1)
                            {
                                nearMissMessage.SetActive(true);
                                advantageBarManager.HandleAttack("NearMiss");
                                Invoke("HideNearMissMessage", 0.2f); // Hides after 0.2 seconds
                                //play attack animation
                                lucienAnimator.SetBool("isAttacking", true);
                                StartCoroutine(ResetAttackAnimation());
                            }
                            else if (combatStateManager.gameState == 2)
                            {
                                nearMissBlockMessage.SetActive(true);
                                advantageBarManager.HandleDefense("Partial");
                                Invoke("HideNearMissBlockMessage", 0.2f); // Hides after 0.2 seconds
                            }

                        }

                        // Break the loop to ensure only one note is hit per press
                        break;
                    }
                }
            }

            if (AudioSettings.dspTime >= nextBeatTime) // Schedule the next beat only when the current DSP time is beyond the next beat
            {
                ScheduleNextBeat();
            }

            //code for tutorial messages
            if(combatStateManager.currentSong.songID == 000)
            {
                handleTutorialMessage();
            }
        }

    }

    //game initialization. Set up the song before playing
    public void startSong()
    {
        songStarted = true; 
        songBPM = combatStateManager.currentSong.BPM; //carry over BPM
        currentBeat = 0;

        crotchet = 60.0f / songBPM; // calculate uration of a single beat
        noteSpawner.setNoteSpeed(); // update note speed based on BPM

        //calculate number of attack/defend notes for advantage bar scaling
        // Get total number of attack notes
        int totalAttackNotes = combatStateManager.currentSong.attackBeatsToHit.Count;

        // Get total number of defense notes
        int totalDefenseNotes = 0;
        foreach (var phase in combatStateManager.currentSong.defendBeatsToHit)
        {
            totalDefenseNotes += phase.Count;
        }
        advantageBarManager.InitializeBar(totalAttackNotes, totalDefenseNotes);

        combatStateManager.currentSong.PlaySong(songManager); //play the song
        dspTimeSongStart = AudioSettings.dspTime; // save reference time for future calculations

        nextBeatTime = dspTimeSongStart + combatStateManager.currentSong.offset; //next beat sound time
    }

    //---------------------------playing beat sounds every whole beat-----------------------------
    void ScheduleNextBeat()
    {
        //audioManager.playBeatSound(nextBeatTime);
        //make characters bounce
        combatStateManager.ApplyBounce();
        combatStateManager.modeText.text = currentBeat.ToString(); ;
    //if (currentBeat == 8) currentBeat = 0;
    currentBeat++;

        
        Debug.Log(currentBeat);

        nextBeatTime += crotchet;
    }

    //------------------------------checking if notes need to be spawned, and spawn them-----------------------------
    int processingDefendList = 0; // Turn to 1 when processing a defend list
    int currentDefendListSize = 0;
    List<BeatData> currentListCopy;
    float transitionBeatTime;

    void CheckAndSpawnNotes()
    {   
        if (combatStateManager.currentSong.attackBeatsToHit.Count > 0 || combatStateManager.currentSong.defendBeatsToHit.Count > 0)
        {
            BeatData nextBeat;
            double dspTimeForNoteSpawn;

            switch (combatStateManager.gameState)
            {
                case 1:  // Attack Mode
                    if (combatStateManager.currentSong.attackBeatsToHit.Count > 0)
                    {
                        nextBeat = combatStateManager.currentSong.attackBeatsToHit[0]; // Get the first beat for attack mode

                        dspTimeForNoteSpawn = GetDspTimeForBeat(nextBeat.beatTime - 4);  // Calculate spawn time, 4 beats before the current beat

                        if (AudioSettings.dspTime >= dspTimeForNoteSpawn && AudioSettings.dspTime < dspTimeForNoteSpawn + crotchet)
                        {
                            Note createdNote = noteSpawner.SpawnNote(nextBeat);
                            activeNotes.Add(createdNote);

                            combatStateManager.currentSong.attackBeatsToHit.RemoveAt(0); // Remove the beat from the list after it has been processed
                        }
                    }
                    break;

                case 2:  // Defend Mode
                    if (combatStateManager.currentSong.defendBeatsToHit.Count > 0)
                    {
                        // If we are not already processing a list, store its size
                        if (processingDefendList == 0)
                        {
                            processingDefendList = 1;  // Mark as processing

                            currentDefendListSize = combatStateManager.currentSong.defendBeatsToHit[0].Count;  // Store list size
                            currentListCopy = new List<BeatData>(combatStateManager.currentSong.defendBeatsToHit[0]);
                            //store the transitioning time (last beat on the list + 1 beat
                            BeatData lastBeat = combatStateManager.currentSong.defendBeatsToHit[0][currentDefendListSize - 1];
                            transitionBeatTime = lastBeat.beatTime - 3;
                        }

                        // Check if the list is now empty
                        if (combatStateManager.currentSong.defendBeatsToHit[0].Count == 0)
                        {
                            // Check if it's time to play the transition sound
                            if (AudioSettings.dspTime >= GetDspTimeForBeat(transitionBeatTime) &&
                                AudioSettings.dspTime < GetDspTimeForBeat(transitionBeatTime) + crotchet)
                            {
                                Debug.Log("defend mode: transitioning");
                                audioManager.playEndEnemyNoteSpawnSound();

                                combatStateManager.currentSong.defendBeatsToHit.RemoveAt(0);
                                processingDefendList = 0; // Reset processing flag
                                currentDefendListSize = 0; // Reset list size
                                transitionBeatTime = 0; // Reset transition time
                            }

                        }else
                        {
                            nextBeat = combatStateManager.currentSong.defendBeatsToHit[0][0]; // Get the first beat for defend mode

                            dspTimeForNoteSpawn = GetDspTimeForBeat(nextBeat.beatTime - 4);  // Calculate spawn time, 8 beats before the current beat

                            if (AudioSettings.dspTime >= dspTimeForNoteSpawn && AudioSettings.dspTime < dspTimeForNoteSpawn + crotchet)
                            {
                                int position = currentDefendListSize - combatStateManager.currentSong.defendBeatsToHit[0].Count + 1; // Get note position

                                Note createdNote = noteSpawner.SpawnDefendNote(nextBeat, position, currentListCopy, transitionBeatTime, transitionBeatTime + 4);
                                audioManager.playEnemyNotePopSound();
                                activeNotes.Add(createdNote);

                                combatStateManager.currentSong.defendBeatsToHit[0].RemoveAt(0);
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
        return dspTimeSongStart + (beatTime * crotchet) + combatStateManager.currentSong.offset;
    }

    //------------------------------for tutorial messages-----------------------------
    public void handleTutorialMessage()
    {
        switch (currentBeat)
        {
            case 0:
                tutorialAMessage.SetActive(true);
                break;
            case 12:
                tutorialAMessage.SetActive(false);
                tutorialBMessage.SetActive(true);
                break;
            case 24:
                tutorialBMessage.SetActive(false);
                tutorialCMessage.SetActive(true);
                break;
            case 36:
                tutorialCMessage.SetActive(false);  
                break;
            case 40:
                tutorialDMessage.SetActive(true);
                break;
            case 84:
                tutorialDMessage.SetActive(false);
                break;
            case 87:
                tutorialEMessage.SetActive(true);
                break;
            case 92:
                tutorialEMessage.SetActive(false);
                tutorialFMessage.SetActive(true);
                break;
            case 100:
                tutorialFMessage.SetActive(false);
                break;
            case 104:
                tutorialGMessage.SetActive(true);
                break;
            case 108:
                tutorialGMessage.SetActive(false);
                tutorialHMessage.SetActive(true);
                break;
            case 112:
                tutorialGMessage.SetActive(true);
                tutorialHMessage.SetActive(false);
                break;
            case 116:
                tutorialGMessage.SetActive(false);
                tutorialHMessage.SetActive(true);
                break;
            case 120:
                tutorialGMessage.SetActive(true);
                tutorialHMessage.SetActive(false);
                break;
            case 124:
                tutorialGMessage.SetActive(false);
                tutorialHMessage.SetActive(true);
                break;
            case 128:
                tutorialGMessage.SetActive(true);
                tutorialHMessage.SetActive(false);
                break;
            case 132:
                tutorialGMessage.SetActive(false);
                tutorialHMessage.SetActive(true);
                break;
            case 136:
                tutorialGMessage.SetActive(true);
                tutorialHMessage.SetActive(false);
                break;
            case 140:
                tutorialGMessage.SetActive(false);
                tutorialHMessage.SetActive(true);
                break;
            case 144:
                tutorialHMessage.SetActive(false);
                break;



        }
    }
    public void hideAllTutorialMessage()
    {
        tutorialAMessage.SetActive(false);
        tutorialBMessage.SetActive(false);
        tutorialCMessage.SetActive(false);
        tutorialDMessage.SetActive(false);
        tutorialEMessage.SetActive(false);
        tutorialFMessage.SetActive(false);
        tutorialGMessage.SetActive(false);
        tutorialHMessage.SetActive(false);
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

    //UI active control

    void ActivateShield(GameObject shield)
    {
        // Deactivate all shields
        redShield.SetActive(false);
        greenShield.SetActive(false);
        purpleShield.SetActive(false);

        // Activate the selected shield
        shield.SetActive(true);

        // Schedule the shield to deactivate after 0.1 seconds
        Invoke(nameof(HideActiveShield), 0.1f);
    }

    void HideActiveShield()
    {
        redShield.SetActive(false);
        greenShield.SetActive(false);
        purpleShield.SetActive(false);
    }

    void HideHighlightedHitArea()
    {
        highLightedHitArea.SetActive(false);
    }

    void HidePerfectHitMessage()
    {
        perfectHitMessage.SetActive(false);
    }
    void HideNearMissMessage()
    {
        nearMissMessage.SetActive(false);
    }

    void HidePerfectBlockMessage()
    {
        perfectBlockMessage.SetActive(false);
    }
    void HideNearMissBlockMessage()
    {
        nearMissBlockMessage.SetActive(false);
    }

    ////////////////////////////////////////////////ANIMATION RELATED ////////////////////////////////////////////////////
    private IEnumerator ResetAttackAnimation()
    {
        // Wait until the attack animation is playing and it's completed
        yield return new WaitForSeconds(lucienAnimator.GetCurrentAnimatorStateInfo(0).length);

        // Set isAttack to false after the animation is done
        lucienAnimator.SetBool("isAttacking", false);
    }

    private IEnumerator ResetDefendAnimation()
    {
        // Wait until the attack animation is playing and it's completed
        yield return new WaitForSeconds(lucienAnimator.GetCurrentAnimatorStateInfo(0).length);

        // Set isAttack to false after the animation is done
        lucienAnimator.SetBool("isDefending", false);
    }

    public void playHurtAnimation()
    {
        lucienAnimator.SetBool("isHurt", true);

        // Get the player's SpriteRenderer and store the original color
        SpriteRenderer spriteRenderer = combatStateManager.player.GetComponent<SpriteRenderer>();

        // Change color to red
        spriteRenderer.color = Color.red;

        StartCoroutine(ResetHurtAnimation(spriteRenderer));
    }

    private IEnumerator ResetHurtAnimation(SpriteRenderer spriteRenderer)
    {
        // Wait until the hurt animation is completed
        yield return new WaitForSeconds(lucienAnimator.GetCurrentAnimatorStateInfo(0).length);

        // Restore original color and reset animation flag
        spriteRenderer.color = Color.white;
        lucienAnimator.SetBool("isHurt", false);
    }

}