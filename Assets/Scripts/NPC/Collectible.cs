using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : NPC
{
    public int itemID;
    public PlayerManager playerManager;

    // Start is called before the first frame update
    void Start()
    {
        playerManager = FindObjectOfType<PlayerManager>();

        if (itemID != 0 && playerManager.GetOwnedItems().Contains(itemID))
        {
            Destroy(gameObject);
        }
    }

    public override void Interact() // Trigger dialogue
    {
        isInteracting = true;
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

    public override void CompleteInteraction()
    {
        isInteracting = false;
        hasInteracted = true;

        if(itemID != 0){
            playerManager.AddOwnedItem(itemID);

        }

        if (shouldDestroy) Destroy(gameObject);
    }
}
