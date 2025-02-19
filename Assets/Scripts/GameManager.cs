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

    public bool hasCompletedIntro, hasCompletedPuzzle1, hasCompletedPuzzle2, hasCompletedPuzzle3;
    public bool hasCompletedCombat1, hasCompletedCombat2, hasCompletedCombat3;
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }


    // Start is called before the first frame update
    void Start()
    {
        SetGameState(GameState.Menu);
    }

    // Update is called once per frame
    void Update()
    {

        
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

}
