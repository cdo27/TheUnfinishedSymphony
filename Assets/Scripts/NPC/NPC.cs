using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NPC : Interactable
{
    public bool shouldLoadScene = false; //load combat after dialogue
    public bool shouldDestroy = false;
    public string sceneToLoad;  
    public Sprite portraitSprite;
    public Dialogue dialogue;
    public GameManager gameManager;

    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();

        if (gameManager == null)
        {
            Debug.Log("GameManager was not found.");
        }
    }
    

    public override void Interact() //trigger dialogue
    {   
        Debug.Log("Playing dialogue");
        isInteracting = true;
        DialogueManager dialogueManager = FindObjectOfType<DialogueManager>();
        dialogueManager.StartDialogue(dialogue, portraitSprite, this);

    }

    public virtual void CompleteInteraction()
    {
        isInteracting = false;
        hasInteracted = true;

        if (shouldLoadScene && !string.IsNullOrEmpty(sceneToLoad))
        {
            SceneManager.LoadScene(sceneToLoad, LoadSceneMode.Additive);
            gameManager.SetGameState(GameManager.GameState.Combat); //update gamestate to combat

        }

        if (shouldDestroy) Destroy(gameObject);
    }

}
