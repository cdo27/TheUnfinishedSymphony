using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneManager : MonoBehaviour
{
    public NPC TutorialAldric; //after puzzle tutorial

    public NPC TutorialThief; //beginning of hallway cutscene

    // void Awake()
    // {
    //     DontDestroyOnLoad(gameObject);
    // }

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
    { //play after player thief combat
    
    }
}
