using UnityEngine;
using UnityEngine.SceneManagement;

public class PuzzleBox : NPC
{
    public new PuzzleLevelConfig levelConfig; 

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
        else
        {
            if (levelConfig != null)
            {
                if (gameManager.CheckPuzzleComplete(levelConfig))
                {
                    Destroy(gameObject);
                }
                else
                {
                    //Debug.Log($"Puzzle {levelConfig.name} is not yet completed.");
                }
            }
        }
    }

    private void Update() //check if complete then destroy
    {
        bool complete = gameManager.CheckPuzzleComplete(levelConfig);
        if(shouldDestroy && complete) Destroy(gameObject);
    }

    public override void Interact()
    {
        isInteracting = true;

        if (isCompleted)
        {
            dialogueManager.StartDialogue(afterDialogue, portraitSprite, this);
            if (displayImage) uiManager.displayPuzzleImage();
        }
        else
        {
            dialogueManager.StartDialogue(dialogue, portraitSprite, this);

            if (levelConfig != null)
            {
                gameManager.SetPuzzleLevelConfig(levelConfig); // Store the level in GameManager
                Debug.Log($"Stored level config {levelConfig.name} in GameManager");
            }
        }
    }

    public override void CompleteInteraction()
    {
        isInteracting = false;
        hasInteracted = true;


        if (shouldLoadScene && !string.IsNullOrEmpty(sceneToLoad))
        {
            SceneManager.LoadScene(sceneToLoad, LoadSceneMode.Additive);
            gameManager.SetGameState(GameManager.GameState.Puzzle);
        }

        if (displayImage)
            uiManager.hidePuzzleImage();
        // if (shouldDestroy)
        //     Destroy(gameObject);
    }
}
