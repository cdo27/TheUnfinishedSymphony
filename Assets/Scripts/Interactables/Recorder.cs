using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Recorder : Interactable
{
    public RecorderDialogue dialogue;
    
    private Sprite[] npcPortraits = new Sprite[3]; // Array for 3 NPC portraits
    
    private RecorderPlay recorderPlay;
    private NPC npcComponent;

    void Start()
    {
        recorderPlay = FindObjectOfType<RecorderPlay>();
        if (recorderPlay == null)
        {
            Debug.LogError("No RecorderPlay found in scene!");
        }
        
        npcComponent = GetComponent<NPC>();
    }

    public override void Interact()
    {
        base.Interact();
        
        if (!isInteracting && recorderPlay != null)
        {
            recorderPlay.StartDialogue(dialogue, npcPortraits, npcComponent);
            isInteracting = true;
        }
    }

    private void OnValidate()
    {
        if (dialogue != null)
        {
            if (dialogue.sentences != null && 
                (dialogue.isPlayerSpeaking.Length != dialogue.sentences.Length ||
                 dialogue.isAldricSpeaking.Length != dialogue.sentences.Length))
            {
                Debug.LogWarning("Dialogue arrays length mismatch in " + gameObject.name);
            }
        }
    }
}