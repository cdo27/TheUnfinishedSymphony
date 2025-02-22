using UnityEngine;
using UnityEngine.SceneManagement;

public class DummyPuzzle : NPC
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
            Debug.LogError("GameManager was not found.");
        }
    }

    public override void Interact()
    {
        isInteracting = true;

        if (isCompleted)
        {
            dialogueManager.StartDialogue(afterDialogue, portraitSprite, this);
            if (displayImage)
                uiManager.displayPuzzleImage();
        }
        else
        {
            dialogueManager.StartDialogue(dialogue, portraitSprite, this);
        }
    }

    public override void CompleteInteraction()
    {
        isInteracting = false;
        //hasInteracted = true;


        if (shouldLoadScene && !string.IsNullOrEmpty(sceneToLoad))
        {
            SceneManager.LoadScene(sceneToLoad, LoadSceneMode.Additive);
            gameManager.SetGameState(GameManager.GameState.Puzzle);
        }

        if (displayImage)
            uiManager.hidePuzzleImage();
        if (shouldDestroy)
            Destroy(gameObject);
    }
}
