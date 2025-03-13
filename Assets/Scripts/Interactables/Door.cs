using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : Interactable
{
    public GameManager gameManager; //message saying its locked
    public DialogueManager dialogueManager;
    public Sprite portraitSprite;
    public Sprite lockedSprite;
    public Dialogue dialogue;
    public int doorNumber;
    

    public string sceneToLoad;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        dialogueManager = FindObjectOfType<DialogueManager>();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void Interact()
    {
        if(doorNumber == 1){ //open hallway door
            if(gameManager.hasCompletedPuzzleTut){
                gameManager.lastScene = "Intro";
                SceneManager.LoadScene(sceneToLoad);
            }else{
                dialogueManager.StartDoorDialogue(dialogue, portraitSprite, this);
            }
        }else if(doorNumber == 2){ //open second door
            if(gameManager.hasCompletedPuzzle1 && gameManager.hasCompletedCombat1){
                gameManager.lastScene = "Hallway";
                SceneManager.LoadScene(sceneToLoad);
            }else{
                dialogueManager.StartDoorDialogue(dialogue, portraitSprite, this);
            }
        }else if(doorNumber == 3){ //open third door
            if(gameManager.hasCompletedPuzzle2 && gameManager.hasCompletedCombat2){
                gameManager.lastScene = "Hallway";
                SceneManager.LoadScene(sceneToLoad);
            }else{
                dialogueManager.StartDoorDialogue(dialogue, portraitSprite, this);
            }
        }
        else if(doorNumber == 4){ //open final cutscene door
            if(gameManager.hasCompletedPuzzle3 && gameManager.hasCompletedCombat3){
                gameManager.lastScene = "EscapeHallway";
                SceneManager.LoadScene("FinalCutscene");
            }else{
                gameManager.lastScene = "EscapeHallway";
                SceneManager.LoadScene(sceneToLoad);
            }
        }else if(doorNumber == 5){ //if thief cutscene done, load hallway insetad of hallwaycutscene
            if(gameManager.hasCompletedCombatTut){
                SceneManager.LoadScene("Hallway");
            }else{
                SceneManager.LoadScene(sceneToLoad);
                gameManager.SetGameState(GameManager.GameState.CutScene);
            }
        }else if(doorNumber == 6){ //if all symphony then, lead into escape room
            if(gameManager.hasCompletedPuzzle3 && gameManager.hasCompletedCombat3){
                gameManager.lastScene = "EscapeRoom";
                SceneManager.LoadScene("EscapeRoom");
            }else{
                dialogueManager.StartDoorDialogue(dialogue, portraitSprite, this);
            }
        }else if(doorNumber == 7){ //if all symphony, escape hallway thief cutscene
            if(gameManager.hasCompletedPuzzle3 && gameManager.hasCompletedCombat3){
                gameManager.lastScene = "Hallway";
                SceneManager.LoadScene("EscapeHallwayCutscene");
            }else{
                gameManager.lastScene = "Hallway";
                SceneManager.LoadScene(sceneToLoad);
            }
        }else if(doorNumber == 9){
            gameManager.lastScene = "Hallway";
            SceneManager.LoadScene(sceneToLoad);
        }else if(doorNumber == 10){
            gameManager.lastScene = "FirstWing";
            SceneManager.LoadScene(sceneToLoad);
        }else if(doorNumber == 11){
            gameManager.lastScene = "SecondWing";
            SceneManager.LoadScene(sceneToLoad);
        }else if(doorNumber == 12){
            gameManager.lastScene = "ThirdWing";
            SceneManager.LoadScene(sceneToLoad);
        }else if(doorNumber == 13){
            gameManager.lastScene = "BossRoom";
            SceneManager.LoadScene(sceneToLoad);
        }else if(doorNumber == 14){
            gameManager.lastScene = "EscapeRoom";
            SceneManager.LoadScene(sceneToLoad);
        }
        else if(doorNumber == 0){ //open door
            SceneManager.LoadScene(sceneToLoad);
        }else{
            dialogueManager.StartDoorDialogue(dialogue, portraitSprite, this);
        }

        


        Debug.Log("Interacted with " + gameObject.name);
        

    }
}
