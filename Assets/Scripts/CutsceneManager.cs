using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneManager : MonoBehaviour
{

    //tutorial NPC
    [Header("Tutorial Elements")]
    public NPC TutorialAldric; //after puzzle tutorial

    public NPC TutorialThief; //beginning of hallway cutscene

    //Wing 1
    [Header("Wing 1 Elements")]
    public PuzzleBox puzzle1;
    public CombatNPC OriginalSerpentura; //keep if not complete
    public NPC DummySerpentura; //after combat dialogue

    public NPC monologueWing1; //after completing puzzle and combat monlogue

    //Wing 2
    [Header("Wing 2 Elements")]
    public PuzzleBox puzzle2;
    public CombatNPC OriginalSonatine; //keep if not complete
    public NPC DummySonatine; 
    public NPC monologueWing2;

    //Wing 3
    [Header("Wing 3 Elements")]
    public PuzzleBox puzzle3;
    public CombatNPC OriginalDueterno; //keep if not complete
    public NPC DummyDueterno; 


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
        if(puzzle1 != null){
            puzzle1.isCompleted = true; //set it to true so other dialogue plays when they interact afterwords
            puzzle1.Interact();
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
        if(puzzle2 != null){
            puzzle2.isCompleted = true; //set it to true so other dialogue plays when they interact afterwords
            puzzle2.Interact();
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
        if(puzzle3 != null){
            puzzle3.isCompleted = true; 
            puzzle3.Interact();
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
}
