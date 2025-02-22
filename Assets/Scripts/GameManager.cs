using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/*
Keep track of what puzzle and combat are completed
States: Menu, Game, CutScene, Puzzle, Combat, End
*/

public class GameManager : MonoBehaviour
{
    public enum GameState
    {
        Menu,
        Game,
        CutScene,
        Puzzle,
        Combat,
        End
    }
    public GameState currentState;
    public AudioManager audioManager;
    public CutsceneManager cutsceneManager;
    public int currentSong;

    public bool hasCompletedPuzzleTut, hasCompletedCombatTut,
    hasCompletedPuzzle1, hasCompletedPuzzle2, hasCompletedPuzzle3,
    hasCompletedCombat1, hasCompletedCombat2, hasCompletedCombat3, hasCompletedFinal = false;

    private bool hasTriggeredAfterPuzzleTutCutscene, hasTriggeredAfterCombatTutCutscene,
    hasTriggeredAfterPuzzle1, hasTriggeredAfterCombat1, hasTriggeredWing1Monologue,
    hasTriggeredAfterCombat2, hasTriggeredAfterPuzzle2,hasTriggeredWing2Monologue,
    hasTriggeredAfterPuzzle3, hasTriggeredAfterCombat3, hasTriggeredWing3Monologue = false;

    public PuzzleLevelConfig currentPuzzleLevelConfig; 
    
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;
        ReassignCutsceneManager();
    }


    // Start is called before the first frame update
    void Start()
    {
        ReassignCutsceneManager();
        audioManager = FindObjectOfType<AudioManager>();
        SetGameState(GameState.Menu);

        // Start the intro music as soon as the game begins
        PlayIntroMusic();
    }

    // Update is called once per frame
    void Update()
    {
        if (hasCompletedPuzzleTut && !hasTriggeredAfterPuzzleTutCutscene){
            hasTriggeredAfterPuzzleTutCutscene = true;
            cutsceneManager.afterPuzzleTut();
        }

        if (hasCompletedCombatTut && !hasTriggeredAfterCombatTutCutscene){
            hasTriggeredAfterCombatTutCutscene = true;
            cutsceneManager.afterCombatTut();
        }

        //--------------Wing 1--------------------------------------------
        
        if(hasCompletedPuzzle1 && !hasTriggeredAfterPuzzle1){
            hasTriggeredAfterPuzzle1 = true;
            cutsceneManager.afterPuzzle1();
        }

        if(hasCompletedCombat1 && !hasTriggeredAfterCombat1){
            hasTriggeredAfterCombat1 = true;
            cutsceneManager.afterCombat1();
        }

        //check Hallway scene and Wing 1 monologue
        if (SceneManager.GetActiveScene().name == "Hallway" &&
            hasCompletedPuzzle1 && hasCompletedCombat1 && !hasTriggeredWing1Monologue)
        {
            hasTriggeredWing1Monologue = true;
            cutsceneManager.PlayWing1Monologue();
        }

        //--------------Wing 2--------------------------------------------

        if(hasCompletedPuzzle2 && !hasTriggeredAfterPuzzle2){
            hasTriggeredAfterPuzzle2 = true;
            cutsceneManager.afterPuzzle2();
        }

        if(hasCompletedCombat2 && !hasTriggeredAfterCombat2){
            hasTriggeredAfterCombat2 = true;
            cutsceneManager.afterCombat2();
        }

        //check Hallway scene and Wing 2 monologue
        if (SceneManager.GetActiveScene().name == "Hallway" &&
            hasCompletedPuzzle2 && hasCompletedCombat2 && !hasTriggeredWing2Monologue)
        {
            hasTriggeredWing2Monologue = true;
            cutsceneManager.PlayWing2Monologue();
        }

        //--------------Wing 3--------------------------------------------
        if(hasCompletedPuzzle3 && !hasTriggeredAfterPuzzle3){
            hasTriggeredAfterPuzzle3 = true;
            cutsceneManager.afterPuzzle3();
        }

        if(hasCompletedCombat3 && !hasTriggeredAfterCombat3){
            hasTriggeredAfterCombat3 = true;
            cutsceneManager.afterCombat3();
        }

        //check Hallway scene and Wing 3 monologue
        if (SceneManager.GetActiveScene().name == "Hallway" &&
            hasCompletedPuzzle3 && hasCompletedCombat3 && !hasTriggeredWing3Monologue)
        {
            hasTriggeredWing3Monologue = true;
            cutsceneManager.PlayWing3Monologue();
        }
    }

    public void SetGameState(GameState newState)
    {
        currentState = newState;

    }

    //Scenes

    public void LoadGameScene(string sceneToLoad)
    {
        SceneManager.LoadScene(sceneToLoad, LoadSceneMode.Additive);
    }

    public void UnloadScene(string sceneToUnload){
        SceneManager.UnloadSceneAsync(sceneToUnload);
    }

    public void SetPuzzleLevelConfig(PuzzleLevelConfig config)
    {
        currentPuzzleLevelConfig = config;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log($"Scene loaded: {scene.name}, mode: {mode}");
        ReassignCutsceneManager();

        if (scene.name == "PuzzleScene") // Apply the level configuration in the puzzle scene
        {
            PuzzleMechanism puzzleMechanism = FindObjectOfType<PuzzleMechanism>();
            if (puzzleMechanism != null && currentPuzzleLevelConfig != null)
            {
                puzzleMechanism.SetLevelConfig(currentPuzzleLevelConfig);
            }
            else
            {
                Debug.LogError("PuzzleMechanism or currentPuzzleLevelConfig is missing in PuzzleScene!");
            }
        }
    }

    void ReassignCutsceneManager()
    {
        Debug.Log("Reassigning CutsceneManager...");
        cutsceneManager = FindObjectOfType<CutsceneManager>();
        if (cutsceneManager != null)
        {
            Debug.Log($"Found CutsceneManager on: {cutsceneManager.gameObject.name}");
        }
        else
        {
            Debug.LogError("Could not find CutsceneManager in the scene!");
        }
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded; 
    }

    //Audio
    public void StopIntroMusic()
    {
        if (audioManager != null)
        {
            audioManager.StopIntroMusic();
        }
    }

    public void PlayIntroMusic()
    {
        if (audioManager != null)
        {
            audioManager.playIntroMusic();
        }
        else
        {
            Debug.LogError("AudioManager not found!");
        }
    }


}
