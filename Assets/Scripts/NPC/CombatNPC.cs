using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CombatNPC : NPC
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

        Debug.Log("Playing dialogue");
        dialogueManager.StartDialogue(dialogue, portraitSprite, this);
    }

    public override void CompleteInteraction()
    {
        isInteracting = false;

        if (shouldLoadScene && !string.IsNullOrEmpty(sceneToLoad))
        {
            SceneManager.LoadScene(sceneToLoad, LoadSceneMode.Additive);
            gameManager.SetGameState(GameManager.GameState.Combat); //update gamestate to combat
        }


    }

    public void destroyNPC(){
        if (shouldDestroy) Destroy(gameObject);
    }

}
