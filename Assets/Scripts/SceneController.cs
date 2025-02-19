using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public GameManager gameManager;

    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();

        if (gameManager == null)
        {
            Debug.Log("GameManager was not found.");
        }
    }
    public void ExitCurrentScene()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.UnloadSceneAsync(currentScene.name);

    }

    public void ExitPuzzleScene()
    {
        SceneManager.UnloadSceneAsync("Puzzle");
        gameManager.SetGameState(GameManager.GameState.Game); //back to Game state

    }
}
