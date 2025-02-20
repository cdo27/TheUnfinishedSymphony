using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DisappearingNPC : NPC
{
    public float fadeDuration = 1f;
    public SpriteRenderer spriteRenderer;

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
        if(!hasInteracted){
            isInteracting = true;
            DialogueManager dialogueManager = FindObjectOfType<DialogueManager>();
            dialogueManager.StartDialogue(dialogue, portraitSprite, this);
        }
        
    }

    public override void CompleteInteraction()
    {
        isInteracting = false;
        hasInteracted = true;

        StartCoroutine(FadeAndDestroy());
        
    }


    private IEnumerator FadeAndDestroy()
    {
        if (spriteRenderer != null)
        {
            float time = 0f;
            Color startColor = spriteRenderer.color;

            while (time < fadeDuration)
            {
                time += Time.deltaTime;
                float alpha = Mathf.Lerp(1f, 0f, time / fadeDuration);
                spriteRenderer.color = new Color(startColor.r, startColor.g, startColor.b, alpha);
                yield return null;
            }
            spriteRenderer.color = new Color(startColor.r, startColor.g, startColor.b, 0f);
        }

        Destroy(gameObject);
    }
}
