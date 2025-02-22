using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : Interactable
{
    public GameManager gameManager; //message saying its locked
    public DialogueManager dialogueManager;
    public Sprite portraitSprite;
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
                gameManager.SetGameState(GameManager.GameState.CutScene);
                SceneManager.LoadScene(sceneToLoad);
            }else{
                dialogueManager.StartDoorDialogue(dialogue, portraitSprite, this);
            }
        }else if(doorNumber == 2){ //open second door
            if(gameManager.hasCompletedPuzzle1 && gameManager.hasCompletedCombat1){
                SceneManager.LoadScene(sceneToLoad);
            }else{
                dialogueManager.StartDoorDialogue(dialogue, portraitSprite, this);
            }
        }else if(doorNumber == 3){ //open third door
            if(gameManager.hasCompletedPuzzle2 && gameManager.hasCompletedCombat2){
                SceneManager.LoadScene(sceneToLoad);
            }else{
                dialogueManager.StartDoorDialogue(dialogue, portraitSprite, this);
            }
        }

        if(doorNumber == 0){ //open door
            SceneManager.LoadScene(sceneToLoad);
        }


        Debug.Log("Interacted with " + gameObject.name);
        

    }
}
