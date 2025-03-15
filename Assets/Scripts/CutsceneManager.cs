using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class CutsceneManager : MonoBehaviour
{

    //tutorial NPC
    [Header("Tutorial Elements")]
    public NPC TutorialAldric; //after puzzle tutorial

    public NPC TutorialThief; //beginning of hallway cutscene
    public NPC DummyThief;

    //Wing 1
    [Header("Wing 1 Elements")]
    public PuzzleBox puzzle1;
    public PuzzleBox dummypuzzle1;
    public CombatNPC OriginalSerpentura; //keep if not complete
    public NPC DummySerpentura; //after combat dialogue

    public NPC monologueWing1; //after completing puzzle and combat monlogue

    //Wing 2
    [Header("Wing 2 Elements")]
    public PuzzleBox puzzle2;
    public PuzzleBox dummypuzzle2;
    public CombatNPC OriginalSonatine; //keep if not complete
    public NPC DummySonatine; 
    public NPC monologueWing2;

    //Wing 3
    [Header("Wing 3 Elements")]
    public PuzzleBox puzzle3;
    public PuzzleBox dummypuzzle3;
    public PuzzleBox cabinet;
    public PlayableDirector playableCutscene3;
    public CombatNPC OriginalDueterno; //keep if not complete
    public NPC DummyDueterno;
    public NPC monologueWing3;

    [Header("Final Cutscenes")]
    public NPC BenedictNPC; //played after wing 3
    public GameObject BenedictAfter; //played after benedictnps dialogue ends
    public bool benedictAfterTriggered = false;


    //EscapeHallwayScenes
    public NPC ThiefNPC; //played after wing 3 in escapehallway
    public GameObject FirstMovement;
    public GameObject SecondMovement;

    //Escape Room Scenes
    public GameObject SceneTrigger;
    public NPC BenedictEscapeNPC;
    public NPC AldricEscapeNPC;

    public bool hasTriggeredAldricEscapeDialogue, hasTriggeredCombat3Dialogue;

    //Final Room
    public NPC AldricNPC; //played in ending entrance
    public NPC CombatTrigger1; //Benedict combat
    public NPC CombatTrigger2; //Aldric combat
    public NPC CombatTrigger3; 

    //Ending Triggers
    [Header("Ending Triggers")]
    
    public NPC EndingTrigger1;
    public NPC EndingTrigger2;
    public NPC EndingTrigger3;

    //Endings
    [Header("Endings")]
    
    public NPC Ending1;
    public NPC Ending2;
    public NPC Ending3;

    // private void Awake()
    // {
    //     if (Instance != null && Instance != this)
    //     {
    //         Destroy(Instance.gameObject);
    //     }

    //     Instance = this;
    //     DontDestroyOnLoad(gameObject);
    // }

    // Update is called once per frame
    void Update()
    {
        if(BenedictNPC != null && BenedictNPC.hasInteracted && benedictAfterTriggered != true){
            benedictAfterTriggered = true;
            BenedictAfter.SetActive(true);
        }

        if(ThiefNPC != null){
            if(ThiefNPC.hasInteracted == true){
                playSecondMovement();
            }
        }

        if(BenedictEscapeNPC!=null && !hasTriggeredAldricEscapeDialogue){
            if(BenedictEscapeNPC.hasInteracted == true){
                hasTriggeredAldricEscapeDialogue =true;
                playAldicEscapeDialogue();
            }
        }

        if(AldricEscapeNPC!=null && !hasTriggeredCombat3Dialogue){
            if(AldricEscapeNPC.hasInteracted == true){
                hasTriggeredCombat3Dialogue = true;
                finalCombatTrigger3();
            }
        }

        //Check endings and load intro scene
        if (Ending1 != null && Ending1.hasInteracted)
        {
            Debug.Log("Ending 1 has finished, loading Intro scene.");
            SceneManager.LoadScene("Intro");
        }
        else if (Ending2 != null && Ending2.hasInteracted)
        {
            Debug.Log("Ending 2 has finished, loading Intro scene.");
            SceneManager.LoadScene("Intro");
        }
        else if (Ending3 != null && Ending3.hasInteracted)
        {
            Debug.Log("Ending 3 has been finished, loading Intro scene.");
            SceneManager.LoadScene("Intro");
        }
        
    }

    //Cutscenes

    public void afterPuzzleTut()
    { //play after player has completed puzzle tutorial
        TutorialAldric.Interact(); //play dialogue after puzzle tutorial
    }
    
    public void beforeCombatTut()
    { //play before player thief combat
        if(TutorialThief == null)
        {
            Debug.Log("thief is null!");
        }
        if(TutorialThief != null)
        TutorialThief.Interact();
    }
    
    public void afterCombatTut()
    { 
        if(DummyThief != null){
            DummyThief.Interact();
            Debug.Log("Played after combat tutorial cutscene");
        }else{
            Debug.Log("TutorialThief is null");
        }
        
    
    }

    //--------------Wing 1----------------------

    public void afterPuzzle1(){
        if(dummypuzzle1 != null){
            dummypuzzle1.isCompleted = true; //set it to true so other dialogue plays when they interact afterwords
            dummypuzzle1.Interact();
            Debug.Log("Played after puzzle 1 cutscene");
        }else{
            Debug.Log("Puzzle 1 is null");
        }

    }

    public void afterCombat1(){
        if(DummySerpentura != null){
            OriginalSerpentura.destroyNPC(); //if needed to destroy
            DummySerpentura.Interact();
            Debug.Log("Played after combat 1 cutscene");
        }else{
            Debug.Log("Combat 1 is null");
        }

    }

    //play Wing 1 monologue
    public void PlayWing1Monologue()
    {
        if (monologueWing1 != null)
        {
            monologueWing1.Interact();
            Debug.Log("Played Wing 1 monologue");
        }
    }


    //--------------Wing 2----------------------
    public void afterPuzzle2(){
        if(dummypuzzle2 != null){
            dummypuzzle2.isCompleted = true; //set it to true so other dialogue plays when they interact afterwords
            dummypuzzle2.Interact();
            Debug.Log("Played after puzzle 2 cutscene");
        }else{
            Debug.Log("Puzzle 2 is null");
        }

    }

    public void afterCombat2(){
        if(OriginalSonatine!= null){
            OriginalSonatine.destroyNPC(); //if needed to destroy
            DummySonatine.Interact();
            Debug.Log("Played after combat 2 cutscene");
        }else{
            Debug.Log("Combat 2 is null");
        }

    }

    //play Wing 2 monologue
    public void PlayWing2Monologue()
    {
        if (monologueWing2 != null)
        {
            monologueWing2.Interact();
            Debug.Log("Played Wing 2 monologue");
        }
    }


    //--------------Wing 3----------------------

    public void afterPuzzle3(){
        if(dummypuzzle3 != null){
            dummypuzzle3.isCompleted = true; 
            dummypuzzle3.Interact();
            PlayPuzzle3();
            Debug.Log("Played after puzzle 3 cutscene");
        }else{
            Debug.Log("Puzzle 3 is null");
        }

    }

    public void afterCombat3(){
        if(OriginalDueterno!= null){
            OriginalDueterno.destroyNPC();
            DummyDueterno.Interact();
            Debug.Log("Played after combat 3 cutscene");
        }else{
            Debug.Log("Combat 3 is null");
        }

    }
    //play puzzle cutscene 
    public void PlayPuzzle3()
    {
        if (playableCutscene3 != null)
        {
            playableCutscene3.gameObject.SetActive(true);
            playableCutscene3.Play();
            Debug.Log("Playing puzzle 3 cutscene");
        }
    }

    public void PlayCabinet()
    {
        if (cabinet != null)
        {
            cabinet.Interact();
            Debug.Log("Playing cabinet cutscene");
        }
    }


    //play Wing 3 monologue
    public void PlayWing3Monologue()
    {
        if (monologueWing3 != null)
        {
            monologueWing3.Interact();
            Debug.Log("Played Wing 3 monologue");
        }
    }

    //--------------Final----------------------

    public void playBenedictScene(){ //hallway play benedict custcene after wing 3
        if(BenedictNPC != null){
            BenedictNPC.gameObject.SetActive(true);
            BenedictNPC.Interact();
            Debug.Log("Playing benedict scene");
        }else{
            Debug.Log("Benedict scene is null");
        }

    }

    public void playThiefScene(){
        if(ThiefNPC != null){
            ThiefNPC.Interact();
            Debug.Log("Playing escape thief scene");
        }else{
            Debug.Log("escape thief scene is null");
        }

    }
    public void playSecondMovement(){
        if(SecondMovement != null){
            FirstMovement.gameObject.SetActive(false);
            SecondMovement.gameObject.SetActive(true);
            Debug.Log("Playing second movement");
        }else{
            Debug.Log("second movement is null");
        }

    }

    //EscapeRoom
    public void playEscapeRoomScene(){
        if(BenedictEscapeNPC != null){
            BenedictEscapeNPC.gameObject.SetActive(true);
            BenedictEscapeNPC.Interact();
        }
    }

    public void playAldicEscapeDialogue(){
        if(AldricEscapeNPC != null){
            AldricEscapeNPC.gameObject.SetActive(true);
            AldricEscapeNPC.Interact();
        }
    }


    //FinalRoom

    public void playAldricScene(){
        if(AldricNPC != null){
            AldricNPC.Interact();
            Debug.Log("Playing aldric final talk");
        }else{
            Debug.Log("aldric scene is null");
        }

    }
    public void finalCombatTrigger1(){ //combat with benedict
        if(CombatTrigger1 != null){
            CombatTrigger1.gameObject.SetActive(true);
            CombatTrigger1.Interact();
            Debug.Log("Playing combat 1");
        }else{
            Debug.Log("combat 1 is null");
        }

    }
    public void finalCombatTrigger2(){ //combat with aldric
        if(CombatTrigger2 != null){
            CombatTrigger2.Interact();
            Debug.Log("Playing combat 2");
        }else{
            Debug.Log("combat 2 is null");
        }

    }

    public void finalCombatTrigger3(){ //combat
        if(CombatTrigger3 != null){
            CombatTrigger3.gameObject.SetActive(true);
            CombatTrigger3.Interact();
            Debug.Log("Playing combat 3");
        }else{
            Debug.Log("combat 3 is null");
        }

    }

    //Play Cutscenes
    public void finalCutscene1(){ //ending cutscene 1
        if(EndingTrigger1 != null){
            EndingTrigger1.Interact();
            Debug.Log("Playing final cutscene 1");
        }else{
            Debug.Log("Final custcene 1 is null");
        }

    }


    public void finalCutscene2(){ //ending cutscene 2 
        if(EndingTrigger2 != null){
            EndingTrigger2.Interact();
            Debug.Log("Playing final cutscene 2");
        }else{
            Debug.Log("Final custcene 2 is null");
        }

    }

    public void finalCutscene3(){ //ending cutscene 3 
        if(EndingTrigger3 != null){
            EndingTrigger3.Interact();
            Debug.Log("Playing final cutscene 3");
        }else{
            Debug.Log("Final custcene 3 is null");
        }

    }

    //Play Cutscene Narration
    public void finalCutsceneNarration1(){ //ending cutscene 1
        if(Ending1 != null){
            Ending1.Interact();
            Debug.Log("Playing final narration 1");
        }else{
            Debug.Log("Final narration 1 is null");
        }

    }


    public void finalCutsceneNarration2(){ //ending cutscene 2 
        if(Ending2 != null){
            Ending2.Interact();
            Debug.Log("Playing final narration 2");
        }else{
            Debug.Log("Final narration 2 is null");
        }

    }

    public void finalCutsceneNarration3(){ //ending cutscene 3 
        if(Ending3 != null){
            Ending3.Interact();
            Debug.Log("Playing final narration 3");
        }else{
            Debug.Log("Final narration 3 is null");
        }

    }


}
