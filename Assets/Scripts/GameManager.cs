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
    public CutsceneManager cutsceneManager;
    public int currentSong;

    public bool hasCompletedPuzzleTut, hasCompletedCombatTut,
    hasCompletedPuzzle1, hasCompletedPuzzle2, hasCompletedPuzzle3,
    hasCompletedCombat1, hasCompletedCombat2, hasCompletedCombat3 = false;

    private bool hasTriggeredAfterPuzzleTutCutscene, hasTriggeredAfterCombatTutCutscene,
    hasTriggeredAfterPuzzle1, hasTriggeredAfterCombat1, hasTriggeredWing1Monologue,
    hasTriggeredAfterCombat2, hasTriggeredAfterPuzzle2 = false;
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
        SetGameState(GameState.Menu);
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

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log($"Scene loaded: {scene.name}, mode: {mode}");
        ReassignCutsceneManager();
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

}
