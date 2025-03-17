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
    public static GameManager Instance;

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
    public string lastScene; //record scene

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


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(Instance.gameObject);
        }

        Instance = this;
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

        //Check for Inventory key presses or Exit 
        if (currentState == GameState.Game)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                Debug.Log("Inventory opened");
                uiManager.ShowInventoryUI();
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Debug.Log("Exit pressed");
                uiManager.ShowExitUI();
            }
        }

        if (hasCompletedPuzzleTut && !hasTriggeredAfterPuzzleTutCutscene){
            hasTriggeredAfterPuzzleTutCutscene = true;
            audioManager.PlaySheetCollectSound();
            cutsceneManager.afterPuzzleTut();
            uiManager.UpdateSymphonyProgress(1);
        }

        if (hasCompletedCombatTut && !hasTriggeredAfterCombatTutCutscene){
            audioManager.PlaySheetCollectSound();
            hasTriggeredAfterCombatTutCutscene = true;
            cutsceneManager.afterCombatTut();
        }

        //--------------Wing 1--------------------------------------------
        
        if(hasCompletedPuzzle1 && !hasTriggeredAfterPuzzle1){
            hasTriggeredAfterPuzzle1 = true;
            audioManager.PlaySheetCollectSound();
            cutsceneManager.afterPuzzle1();
        }

        if(hasCompletedCombat1 && !hasTriggeredAfterCombat1){
            hasTriggeredAfterCombat1 = true;
            audioManager.PlaySheetCollectSound();
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
            audioManager.PlaySheetCollectSound();
            cutsceneManager.afterPuzzle2();
        }

        if(hasCompletedCombat2 && !hasTriggeredAfterCombat2){
            hasTriggeredAfterCombat2 = true;
            audioManager.PlaySheetCollectSound();
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
            audioManager.PlaySheetCollectSound();
            cutsceneManager.afterPuzzle3();
        }

        if(hasCompletedCombat3 && !hasTriggeredAfterCombat3){
            hasTriggeredAfterCombat3 = true;
            audioManager.PlaySheetCollectSound();
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
            }else{
                cutsceneManager.finalCutscene3();
                Debug.Log("Play ending 3");
            }
        }

        if (currentState == GameState.Combat || currentState == GameState.Puzzle)
        {   
            audioManager?.StopDialogueSFX();
            audioManager?.StopWalkingSound();
        }
    }

    public void SetGameState(GameState newState)
    {
        currentState = newState;

    }

    //Scenes
    // Define Wing Scenes
    private HashSet<string> scenesWithSFX = new HashSet<string>
    {
        "FirstWing", "SecondWing", "ThirdWing", // Example: Wing scenes
        "Hallway", "HallwayCutscene"               // Add more if needed
    };

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

        //  Play Scene Transition Sound ONLY for specific scenes
        if (audioManager != null && scenesWithSFX.Contains(scene.name))
        {
            audioManager.PlayWingSFX();
        }

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

    public bool CheckCombatComplete(int songID)
    {
        switch (songID)
        {
            case 2:
                return hasCompletedCombat1;
            case 3:
                return hasCompletedCombat2;
            case 4:
                return hasCompletedCombat3;
            default:
                return false;
        }
    }

    public void PlayerPassedArea()
    {
        Debug.Log("Triggering scene");
        cutsceneManager.playEscapeRoomScene();
    }

    


}
