using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Recorder : Interactable
{
    public Sprite portraitSprite;
    public RecorderDialogue dialogue;
    public RecorderDialogue afterDialogue;
    public GameManager gameManager;
    public RecorderManager recorderManager;
    public UIManager uiManager;

    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
        recorderManager = FindObjectOfType<RecorderManager>();
        uiManager = FindObjectOfType<UIManager>();
   
        if (gameManager == null)
        {
            Debug.LogError("GameManager was not found.");
        }
    }

    public override void Interact()
    {   
        isInteracting = true;

        if (!hasInteracted)
        {
            Debug.Log("Playing dialogue");
            recorderManager.StartDialogue(dialogue, portraitSprite, this);
        }
        else
        {
            Debug.Log("Playing after dialogue");
            recorderManager.StartDialogue(afterDialogue, portraitSprite, this);
        }
    }

    public virtual void CompleteInteraction()
    {
        isInteracting = false;
        hasInteracted = true;
    }
}

