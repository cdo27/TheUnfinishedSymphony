using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThiefNPC : NPC
{
    public Dialogue repeatDialogue;
    public GameObject shopIndicator;

    private bool shopIndicatorActivated = false; 

    void Start()
    {
        uiManager = FindObjectOfType<UIManager>();

        if (shopIndicator != null)
        {
            shopIndicator.SetActive(false);
        }
    }

    void Update()
    {
        if (gameManager.hasCompletedCombatTut && !shopIndicatorActivated)
        {
            ActivateShopIndicator();
        }
    }

    private void ActivateShopIndicator()
    {
        if (shopIndicator != null)
        {
            shopIndicator.SetActive(true);
            shopIndicatorActivated = true;
        }
    }

    public override void Interact()
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
