using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NPC : Interactable
{
    public bool shouldLoadScene = false; // Load combat or other scenes after dialogue
    public bool shouldDestroy = false; // Destroy the NPC after interaction
    public string sceneToLoad;
    public Sprite portraitSprite;
    public Dialogue dialogue;
    public Dialogue afterDialogue;
    public GameManager gameManager;
    public DialogueManager dialogueManager;
    public UIManager uiManager;
    private PuzzleMechanism puzzleMechanism; // Reference to the puzzle mechanism

    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
        dialogueManager = FindObjectOfType<DialogueManager>();
        uiManager = FindObjectOfType<UIManager>();
        puzzleMechanism = FindObjectOfType<PuzzleMechanism>(); // Find the PuzzleMechanism in the scene

        if (gameManager == null)
        {
            Debug.LogError("GameManager was not found.");
        }
    }

    public override void Interact() // Trigger dialogue
    {   
        isInteracting = true;
        gameManager.SetGameState(GameManager.GameState.Combat); 
        if (!hasInteracted)
        {
            Debug.Log("Playing dialogue");
            dialogueManager.StartDialogue(dialogue, portraitSprite, this);
        }
        else
        {
            Debug.Log("Playing after dialogue");
            dialogueManager.StartDialogue(afterDialogue, portraitSprite, this);
        }
    }

    public virtual void CompleteInteraction()
    {
        isInteracting = false;
        hasInteracted = true;

        if (shouldLoadScene && !string.IsNullOrEmpty(sceneToLoad))
        {   
            SceneManager.LoadScene(sceneToLoad, LoadSceneMode.Additive);

        }

        if (shouldDestroy) Destroy(gameObject);
    }
}

