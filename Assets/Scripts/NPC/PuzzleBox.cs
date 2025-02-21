using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PuzzleBox : NPC
{

    public bool isCompleted = false;
    public bool displayImage = false;

    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
        dialogueManager = FindObjectOfType<DialogueManager>();
        uiManager = FindObjectOfType<UIManager>();

        if (gameManager == null)
        {
            Debug.Log("GameManager was not found.");
        }
    }


    public override void Interact() //trigger dialogue
    {   
        isInteracting = true;
        
        if(isCompleted)
        {
            Debug.Log("Playing after dialogue");
            dialogueManager.StartDialogue(afterDialogue, portraitSprite, this);

            if(displayImage){ //if there is something to display
                uiManager.displayPuzzleImage();
            }
        }
        else{
            Debug.Log("Playing dialogue");
            dialogueManager.StartDialogue(dialogue, portraitSprite, this);
        }

    }

    public override void CompleteInteraction()
    {
        isInteracting = false;
        //hasInteracted = true;

        if (shouldLoadScene && !string.IsNullOrEmpty(sceneToLoad))
        {
            SceneManager.LoadScene(sceneToLoad, LoadSceneMode.Additive);
            gameManager.SetGameState(GameManager.GameState.Combat); //update gamestate to combat
        }

        if(displayImage) uiManager.hidePuzzleImage(); //hide if ther is a diplay image

        if (shouldDestroy) Destroy(gameObject);
    }

}
