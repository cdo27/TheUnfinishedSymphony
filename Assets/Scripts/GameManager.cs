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
    public UIManager uiManager;
    public int currentSong;

    public bool hasCompletedPuzzleTut, hasCompletedCombatTut,
    hasCompletedPuzzle1, hasCompletedPuzzle2, hasCompletedPuzzle3,
    hasCompletedCombat1, hasCompletedCombat2, hasCompletedCombat3, hasCompletedFinal = false;

    private bool hasTriggeredAfterPuzzleTutCutscene, hasTriggeredAfterCombatTutCutscene,
    hasTriggeredAfterPuzzle1, hasTriggeredAfterCombat1, hasTriggeredWing1Monologue,
    hasTriggeredAfterCombat2, hasTriggeredAfterPuzzle2,hasTriggeredWing2Monologue,
    hasTriggeredAfterPuzzle3, hasTriggeredAfterCombat3, hasTriggeredWing3Monologue,
    hasTriggeredBenedictScene, hasTriggeredChoiceCombat, hasTriggeredFinal = false;

    public bool hasMadeChoice, Ending1, Ending2, Ending3 = false; //which ending

    public PuzzleLevelConfig currentPuzzleLevelConfig; 
    public PuzzleLevelConfig TutPuzzle;
    public PuzzleLevelConfig PuzzleOne;
    public PuzzleLevelConfig PuzzleTwo;
    public PuzzleLevelConfig PuzzleThree;
    
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
        SetGameState(GameState.Game);

        // Start the intro music as soon as the game begins
        PlayIntroMusic();
    }

    // Update is called once per frame
    void Update()
    {
        if (hasCompletedPuzzleTut && !hasTriggeredAfterPuzzleTutCutscene){
            hasTriggeredAfterPuzzleTutCutscene = true;
            cutsceneManager.afterPuzzleTut();
            uiManager.UpdateSymphonyProgress(1);
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

        if(hasTriggeredAfterCombat1 && hasTriggeredAfterPuzzle1){
            uiManager.UpdateSymphonyProgress(2);
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

        if(hasTriggeredAfterCombat2 && hasTriggeredAfterPuzzle2){
            uiManager.UpdateSymphonyProgress(3);
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

        if(hasTriggeredAfterCombat3 && hasTriggeredAfterPuzzle3){
            uiManager.UpdateSymphonyProgress(4);
        }

        //check third wing scene and Wing 3 monologue
        if (SceneManager.GetActiveScene().name == "ThirdWing" &&
            hasCompletedPuzzle3 && hasCompletedCombat3 && !hasTriggeredWing3Monologue)
        {
            hasTriggeredWing3Monologue = true;
            cutsceneManager.PlayWing3Monologue();
        }

        //BenedictScene
        if (SceneManager.GetActiveScene().name == "Hallway" &&
            hasCompletedPuzzle3 && hasCompletedCombat3 && !hasTriggeredBenedictScene)
        {
            hasTriggeredBenedictScene = true;
            cutsceneManager.playBenedictScene();
            Debug.Log("Playing benedict scene");
        }

        //Finals
        if(hasMadeChoice == true & !hasTriggeredChoiceCombat){ //after choice, final combat
            hasTriggeredChoiceCombat = true;
            if (Ending1){
                cutsceneManager.finalCombatTrigger1();
                Debug.Log("Play combat 1");
            }else if (Ending2){
                cutsceneManager.finalCombatTrigger2();
                Debug.Log("Play combat 2");
            }
        }

        if(hasCompletedFinal && !hasTriggeredFinal){ //after battle
            hasTriggeredFinal = true;
            if (Ending1){
                cutsceneManager.finalCutscene1();
                Debug.Log("Play ending 1");
            }else if (Ending2){
                cutsceneManager.finalCutscene2();
                Debug.Log("Play ending 2");
            }else if (Ending3){
                Debug.Log("Play ending 3");
            }
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

    public bool CheckPuzzleComplete(PuzzleLevelConfig levelConfig){
        if (levelConfig == PuzzleOne){
            if(hasCompletedPuzzle1) return true;
        }else if (levelConfig == PuzzleTwo){
            if(hasCompletedPuzzle2) return true;
        }else if (levelConfig == PuzzleThree){
            if(hasCompletedPuzzle3) return true;
        }else if (levelConfig == TutPuzzle){
            if(hasCompletedPuzzleTut) return true;
        }
        return false;
    }

    


}
