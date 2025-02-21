using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThiefNPC : NPC
{
    public Dialogue repeatDialogue;

    void Start()
    {
        //find UIManager in the current scene
        uiManager = FindObjectOfType<UIManager>();
    }

    public override void Interact() //trigger dialogue
    {   
        isInteracting = true;
        if (!hasInteracted)
        {
            FindObjectOfType<DialogueManager>().StartDialogue(dialogue, portraitSprite, this);
        }
        else
        {
            FindObjectOfType<DialogueManager>().StartDialogue(repeatDialogue, portraitSprite, this);
        }
        
    }

    public override void CompleteInteraction()
    {
        isInteracting = false;
        hasInteracted = true;
        uiManager.ShowShopUI();
    }
}
