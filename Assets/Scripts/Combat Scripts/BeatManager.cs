using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Collections;
using Unity.VisualScripting;
using static UnityEngine.Rendering.DebugUI;

public class BeatManager : MonoBehaviour
{
    //---------------------------initiate all the necessary objects and components------------------------------------ 
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

    //not hit area lighted up object
    public GameObject highLightedHitArea;
    //magic shield object
    public GameObject redShield;
    public GameObject greenShield;
    public GameObject purpleShield;

    //for result count
    public int[] attackResults = new int[4]; //0 = perfect hit, 1 = near miss, 2 = wrong key, 3 = failed hit
    public int[] defendResults = new int[4]; //0 = perfect guard, 1 = weak guard, 2 = wrong key, 3 = failed guard

    //note hit messages
    public GameObject perfectHitMessage;
    public GameObject nearMissMessage;
    public GameObject failedHitMessage;
    public GameObject wrongKeyMessage;
    public GameObject perfectBlockMessage;
    public GameObject nearMissBlockMessage;
    public GameObject defendWrongKeyMessage;

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
        
    }

    //game initialization. Set up the song before playing
    public void startSong()
    {
        songStarted = true;
        songBPM = combatStateManager.currentSong.BPM; //carry over BPM
        currentBeat = 0;

        crotchet = 60.0f / songBPM; // calculate uration of a single beat
        noteSpawner.setNoteSpeed(); // update note speed based on BPM

        attackResults = new int[4]; 
        defendResults = new int[4]; 

    combatStateManager.initilizeAdvantageBar();

        combatStateManager.currentSong.PlaySong(combatStateManager.songManager); //play the song
        dspTimeSongStart = AudioSettings.dspTime; // save reference time for future calculations

        nextBeatTime = dspTimeSongStart + combatStateManager.currentSong.offset; //next beat sound time
    }

    void Update()
    { 
        //-----------------------------------check for win or lose condition---------------------------------------
        if(songStarted && AudioSettings.dspTime >=  GetDspTimeForBeat(combatStateManager.currentSong.songcompleteBeat))
        {
            if(combatStateManager.gameState != 198 && combatStateManager.advantageBarManager.CheckVictoryCondition() == true)
            {
                combatStateManager.gameState = 98; 
            }
            else if (combatStateManager.gameState != 199 &&  combatStateManager.advantageBarManager.CheckVictoryCondition() == false)
            {
                combatStateManager.gameState = 99;
            }

        }

        //----------------------------------code for gameplay once song started-------------------------------------
        if (songStarted && combatStateManager.gameState != 98 && combatStateManager.gameState != 99)
        {
            CheckAndSpawnNotes(); // Check and spawn notes based on beats before they arrive
            CheckAndDeleteNotes(); // check if there's any destroyed notes, if yes remove from list

            //perfect shield
            if (Input.GetKeyDown(KeyCode.E) && combatStateManager.gameState == 2 && processingDefendList != 0 && combatStateManager.combatItemManager.hasItem4 == true && combatStateManager.combatItemManager.perfectShieldActive == false)
            {
                combatStateManager.combatItemManager.activatePerfectShield();
            }

            // Player hit
            if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow) ||
                Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow) ||
                Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
            {
                // Set up default behavior when the player presses the key during attack and defend mode
                if (combatStateManager.gameState == 1) combatStateManager.audioManager.playHitSoundA();
                if (combatStateManager.gameState == 2)
                {
                    //play defend animation
                    combatStateManager.combatAnimationManager.LucienPlayDefendAnimation();

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

                // Loop through all active notes(inside handle functions) and check if one can be hit
                if (combatStateManager.gameState == 1)
                {
                    HandleAttackNotes(noteHit);
                }
                else if (combatStateManager.gameState == 2)
                {
                    HandleDefendNotes(noteHit);
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

    //handle hits for attack and defend phase
    void HandleAttackNotes(bool noteHit)
    {
        for (int i = activeNotes.Count - 1; i >= 0; i--)
        {
            Note note = activeNotes[i];

            bool correctKeyPressed = (note.beat.noteType == 0 && (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))) ||
                                     (note.beat.noteType == 1 && (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))) ||
                                     (note.beat.noteType == 2 && (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)));


            int hitResult = note.checkIfHit();

            //nothing happens
            if (!correctKeyPressed && hitResult == 0 && hitResult == 3)
            {
                continue;
            }

            //actual handle
            if (correctKeyPressed)
            {
                
                if (!noteHit && (hitResult == 2 || hitResult == 1))
                {
                    noteHit = true;
                    highLightedHitArea.SetActive(true);
                    Invoke("HideHighlightedHitArea", 0.05f);
                    bool isCriticalHit = false;
                    if(combatStateManager.combatItemManager.hasItem5 == true)
                    {
                        isCriticalHit = combatStateManager.combatItemManager.decideIfCriticalHit();
                        if(isCriticalHit == true)
                        {
                            combatStateManager.combatItemManager.FlashingWeaponIcon();
                        }
                    }
                    note.handleAttackHit(combatStateManager.enemyHitPoint.transform.position, isCriticalHit);
                    //perfect hit
                    if (hitResult == 2)
                    {
                        attackResults[0]++;
                        perfectHitMessage.SetActive(true);
                        combatStateManager.advantageBarManager.HandleAttack("Perfect", isCriticalHit);
                        Invoke("HidePerfectHitMessage", 0.15f);
                        combatStateManager.combatAnimationManager.LucienPlayAttackAnimation();
                    }
                    //near miss
                    else if (hitResult == 1)
                    {
                        attackResults[1]++;
                        nearMissMessage.SetActive(true);
                        combatStateManager.advantageBarManager.HandleAttack("NearMiss", isCriticalHit);
                        Invoke("HideNearMissMessage", 0.15f);
                        combatStateManager.combatAnimationManager.LucienPlayAttackAnimation();
                    }
                    break;
                }else if (!noteHit && hitResult == 3)
                {
                    attackResults[3]++;
                    noteHit = true;
                    note.handleFailedHit();

                    failedHitMessage.SetActive(true);
                    Invoke("HideFailedHitMessage", 0.15f);
                    break;
                }
            }

            if (!noteHit &&  !correctKeyPressed && (hitResult == 1 || hitResult == 2))
            {
                attackResults[2]++;
                noteHit = true;
                // Player pressed the wrong key at the right time
                wrongKeyMessage.SetActive(true);
                Invoke("HideWrongKeyMessage", 0.15f);
                note.handleFailedHit();
                break;
            }

        }
    }

    void HandleDefendNotes(bool noteHit)
    {
        for (int i = activeNotes.Count - 1; i >= 0; i--)
        {
            Note note = activeNotes[i];

            bool correctKeyPressed = (note.beat.noteType == 0 && (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))) ||
                                     (note.beat.noteType == 1 && (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))) ||
                                     (note.beat.noteType == 2 && (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)));

            int hitResult = note.checkIfHit();



            //nothing happens
            if (!correctKeyPressed && hitResult == 0 && hitResult == 3)
            {
                continue;
            }

            if (correctKeyPressed)
            {
                //correct hit with correct key
                if (!noteHit && (hitResult == 2 || hitResult == 1))
                {
                    noteHit = true;
                    highLightedHitArea.SetActive(true);
                    Invoke("HideHighlightedHitArea", 0.05f);

                    combatStateManager.audioManager.playMusicBlockSound();
                    note.handleDefendHit(combatStateManager.enemyHitPoint.transform.position);

                    if (hitResult == 2)
                    {
                        defendResults[0]++; // Perfect block
                        perfectBlockMessage.SetActive(true);
                        Invoke("HidePerfectBlockMessage", 0.15f);
                    }
                    else if (hitResult == 1)
                    {
                        defendResults[1]++; // Near miss block
                        nearMissBlockMessage.SetActive(true);
                        combatStateManager.advantageBarManager.HandleDefense("Partial");
                        Invoke("HideNearMissBlockMessage", 0.15f);
                    }
                    break;
                }
            }
            //if wrong key pressed
            if (!noteHit && !correctKeyPressed && (hitResult == 1 || hitResult == 2))
            {
                defendResults[2]++;
                noteHit = true;
                defendWrongKeyMessage.SetActive(true);
                Invoke("HideDefendWrongKeyMessage", 0.15f);
                combatStateManager.advantageBarManager.HandleDefense("Miss");
                combatStateManager.audioManager.playExplosionSound();
                note.defendNoteExplode();
                break;
            }
        }
    }


    //---------------------------playing beat sounds every whole beat-----------------------------
    void ScheduleNextBeat()
    {
        //audioManager.playBeatSound(nextBeatTime);

        //make characters bounce
        combatStateManager.combatAnimationManager.ApplyBounce();
        combatStateManager.modeText.text = currentBeat.ToString(); ;

        currentBeat++;

        
        Debug.Log(currentBeat);

        nextBeatTime += crotchet;
    }

    //------------------------------checking if notes need to be spawned, and spawn them-----------------------------
    int processingDefendList = 0; // Turn to 1 when processing a defend list
    int currentDefendListSize = 0;
    bool enemySwitchToAttack = false;
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
                                combatStateManager.audioManager.playEndEnemyNoteSpawnSound();

                                //and stop animation
                                combatStateManager.combatAnimationManager.EnemyStopAttackAnimation();
                                enemySwitchToAttack = false;
                                //other resets
                                combatStateManager.currentSong.defendBeatsToHit.RemoveAt(0);
                                processingDefendList = 0; // Reset processing flag
                                currentDefendListSize = 0; // Reset list size
                                transitionBeatTime = 0; // Reset transition time
                            }

                        }else
                        {
                           
                            nextBeat = combatStateManager.currentSong.defendBeatsToHit[0][0]; // Get the first beat for defend mode

                            dspTimeForNoteSpawn = GetDspTimeForBeat(nextBeat.beatTime - 4);  // Calculate spawn time, 4 beats before the current beat

                            if (AudioSettings.dspTime >= dspTimeForNoteSpawn && AudioSettings.dspTime < dspTimeForNoteSpawn + crotchet)
                            {
                                if (enemySwitchToAttack != true)
                                {
                                    if (combatStateManager.combatItemManager.hasItem4 == true)  combatStateManager.combatItemManager.deactivatePerfectShield();
                                    enemySwitchToAttack = true;
                                    combatStateManager.combatAnimationManager.EnemyPlayAttackAnimation();
                                }
                                int position = currentDefendListSize - combatStateManager.currentSong.defendBeatsToHit[0].Count + 1; // Get note position

                                Note createdNote = noteSpawner.SpawnDefendNote(nextBeat, position, currentListCopy, transitionBeatTime, transitionBeatTime + 4);
                                combatStateManager.audioManager.playEnemyNotePopSound();

                                // Move note further back in Z-space (negative values bring it forward)
                                createdNote.transform.localPosition += new Vector3(0, 0, position * 0.01f);

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
            case 28:
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
                tutorialGMessage.SetActive(true);
                tutorialHMessage.SetActive(false);
                break;
            case 148:
                tutorialGMessage.SetActive(false);
                tutorialHMessage.SetActive(true);
                break;
            case 152:
                tutorialGMessage.SetActive(true);
                tutorialHMessage.SetActive(false);
                break;
            case 156:
                tutorialGMessage.SetActive(false);
                tutorialHMessage.SetActive(true);
                break;
            case 160:
                tutorialHMessage.SetActive(false);
                tutorialFMessage.SetActive(false);
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

    
    //-------------------------------------------UI active control----------------------------------------------
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

    public void HideActiveShield()
    {
        redShield.SetActive(false);
        greenShield.SetActive(false);
        purpleShield.SetActive(false);
        if (combatStateManager.combatItemManager.hasItem4 == true)
        {
            combatStateManager.combatItemManager.deactivatePerfectShield();
            combatStateManager.combatItemManager.perfectShieldKey.SetActive(false);
        }
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
    void HideFailedHitMessage()
    {
        failedHitMessage.SetActive(false);
    }

    void HideWrongKeyMessage()
    {
        wrongKeyMessage.SetActive(false);
    }

    void HidePerfectBlockMessage()
    {
        perfectBlockMessage.SetActive(false);
    }
    void HideNearMissBlockMessage()
    {
        nearMissBlockMessage.SetActive(false);
    }
    void HideDefendWrongKeyMessage()
    {
        defendWrongKeyMessage.SetActive(false);
    }

  }