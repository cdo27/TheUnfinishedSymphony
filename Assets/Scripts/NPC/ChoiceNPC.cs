using UnityEngine;

public class ChoiceNPC : NPC
{

    public ChoiceDialogue choiceDialogue;
    public override void Interact()
    {
        isInteracting = true;
        Debug.Log("Playing choice dialogue");
        dialogueManager.StartDialogue(choiceDialogue ?? dialogue, portraitSprite, this);

    }

    public override void CompleteInteraction()
    {
        base.CompleteInteraction();
        //update gamemanager with choice and play cutscene
    }
}