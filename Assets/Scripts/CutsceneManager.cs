using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneManager : MonoBehaviour
{
    //tutorial NPC
    public NPC TutorialAldric; //after puzzle tutorial

    public NPC TutorialThief; //beginning of hallway cutscene

    //Wing 1
    public PuzzleBox puzzle1;
    public CombatNPC OriginalSerpentura; //keep if not complete
    public NPC DummySerpentura; //after combat dialogue

    public NPC monologueWing1; //after completing puzzle and combat monlogue

    //Wing 2

    //Wing 3

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
}
