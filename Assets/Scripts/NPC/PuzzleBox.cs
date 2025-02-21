using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PuzzleBox : NPC
{
    public PuzzleLevelConfig levelConfig; 
    public PuzzleMechanism puzzleMechanism; 

    public bool isCompleted = false;
    public bool displayImage = false;

    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
        dialogueManager = FindObjectOfType<DialogueManager>();
        uiManager = FindObjectOfType<UIManager>();
        puzzleMechanism = FindObjectOfType<PuzzleMechanism>(); 

        if (gameManager == null)
        {
            Debug.LogError("GameManager was not found.");
        }

        if (puzzleMechanism == null)
        {
            Debug.LogError("PuzzleMechanism was not found.");
        }
    }

    public override void Interact()
    {
        isInteracting = true;
        
        if (isCompleted)
        {
            Debug.Log("Playing after dialogue");
            dialogueManager.StartDialogue(afterDialogue, portraitSprite, this);

            if (displayImage)
            {
                uiManager.displayPuzzleImage();
            }
        }
        else
        {
            Debug.Log("Playing dialogue");
            dialogueManager.StartDialogue(dialogue, portraitSprite, this);

            if (puzzleMechanism != null && levelConfig != null)
            {
                puzzleMechanism.SetLevelConfig(levelConfig);
            }
        }
    }

    public override void CompleteInteraction()
    {
        isInteracting = false;

        if (shouldLoadScene && !string.IsNullOrEmpty(sceneToLoad))
        {
            SceneManager.LoadScene(sceneToLoad, LoadSceneMode.Additive);
            gameManager.SetGameState(GameManager.GameState.Combat);
        }

        if (displayImage) uiManager.hidePuzzleImage();
        if (shouldDestroy) Destroy(gameObject);
    }
}

