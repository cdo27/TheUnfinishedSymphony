using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public GameManager gameManager;
    public string originalSceneName;
    public string combatSceneName = "Combat";

    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();

        if (gameManager == null)
        {
            Debug.Log("GameManager was not found.");
        }
    }

    public void LoadCutscene(string sceneToLoad)
    {
        SceneManager.LoadScene(sceneToLoad, LoadSceneMode.Additive);
        gameManager.SetGameState(GameManager.GameState.CutScene);

    }
    public void ExitCurrentScene(string sceneToUnload)
    {
        SceneManager.UnloadSceneAsync(sceneToUnload);

    }

    public void ExitPuzzleScene()
    {
        SceneManager.UnloadSceneAsync("Puzzle");
        gameManager.SetGameState(GameManager.GameState.Game); //back to Game state
        gameManager.audioManager.PlayBackgroundMusic();

    }

    public void ExitCombatScene()
    {
        Scene combatScene = SceneManager.GetSceneByName("Combat");
        SceneManager.UnloadSceneAsync(combatScene);
        gameManager.SetGameState(GameManager.GameState.Game); //back to Game state
        gameManager.audioManager.PlayBackgroundMusic();
    }

    public void MoveObject(Note note)
    {
        Scene combatScene = SceneManager.GetSceneByName("Combat");
        if (combatScene.isLoaded){
            SceneManager.MoveGameObjectToScene(note.gameObject, combatScene);
        }else{
            Debug.Log("Move note failed");
        }

    }
}
