using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class CutsceneManager : MonoBehaviour
{

    //tutorial NPC
    [Header("Tutorial Elements")]
    public NPC TutorialAldric; //after puzzle tutorial

    public NPC TutorialThief; //beginning of hallway cutscene

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

    [Header("Final Cutscene")]
    public NPC BenedictNPC;
    public NPC AldricNPC;
    public NPC DummyFinal;

    //Endings
    public NPC EndingTrigger;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Cutscenes

    public void afterPuzzleTut()
    { //play after player has completed puzzle tutorial
        TutorialAldric.Interact(); //play dialogue after puzzle tutorial
    }
    
    public void beforeCombatTut()
    { //play before player thief combat
        if(TutorialThief != null)
        TutorialThief.Interact();
    }
    
    public void afterCombatTut()
    { 
        if(TutorialThief != null){
            TutorialThief.Interact();
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

    public void playBenedictScene(){
        if(BenedictNPC != null){
            BenedictNPC.gameObject.SetActive(true);
            BenedictNPC.Interact();
            Debug.Log("Playing benedict scene");
        }else{
            Debug.Log("Benedict scene is null");
        }

    }

    public void finalTrigger2(){
        if(EndingTrigger != null){
            EndingTrigger.Interact();
            Debug.Log("Playing final cutscene 2");
        }else{
            Debug.Log("Final custcene 2 is null");
        }

    }

    public void finalCutscene2(){
        if(AldricNPC != null){
            AldricNPC.Interact();
            Debug.Log("Playing final cutscene 2");
        }else{
            Debug.Log("Final custcene 2 is null");
        }

    }


}
