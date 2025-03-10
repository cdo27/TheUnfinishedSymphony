using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CombatStateManager : MonoBehaviour
{
    //current song that will be played, when loading combat this should be loaded based on which level
    public Song currentSong;

    public int gameState = 0; // 0 = not started, 1 = attack mode, 2 = defend mode, 98 = victory, 99 = defeat
    public double lastCheckedTime = -1.0; // Track last checked DSP time

    public SceneController sceneController;
    public GameManager gameManager;
    public BeatManager beatManager;
    public AdvantageBarManager advantageBarManager;
    public CombatAnimationManager combatAnimationManager;
    public SongManager songManager;
    public AudioManager audioManager;

    public GameObject attackBar;

    public GameObject attackModeBanner;
    public GameObject defendModeBanner;

    public GameObject StartScreen;
    public GameObject StartButton;
    public GameObject VictoryScreen;
    public GameObject victoryContinueButton;
    public GameObject DefeatScreen;

    public GameObject player;
    public GameObject enemy;
    //the enemy hit point 
    public GameObject enemyHitPoint;

    public Sprite thiefSprite;
    public Sprite redSpiritSprite;
    public Sprite yellowSpiritSprite;
    public Sprite blueSpiritSprite;
    public Sprite aldricSprite;

    // Just for testing
    public TMP_Text modeText;

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        sceneController = FindObjectOfType<SceneController>();

        if (gameManager != null && gameManager.audioManager != null)
        {
            gameManager.audioManager.StopBackgroundMusic();
            gameManager.SetGameState(GameManager.GameState.Combat);
        }
        // Get AudioManager reference
        audioManager = FindObjectOfType<AudioManager>();
        if (audioManager == null)
        {
            Debug.LogError("AudioManager not found in the scene!");
        }

        // Get AudioManager reference
        songManager = FindObjectOfType<SongManager>();
        if (songManager == null)
        {
            Debug.LogError("AudioManager not found in the scene!");
        }


        //set up the proper song
        selectSong();
        StartScreen.SetActive(true);
        //set up the sprites and animators for enemies
        //Animator enemyAnimator = enemy.GetComponent<Animator>();
        SpriteRenderer enemySpriteRenderer = enemy.GetComponent<SpriteRenderer>();
        if (currentSong.songID == 001)
        {
            combatAnimationManager.setEnemyAnimator(1);
            enemySpriteRenderer.sprite = thiefSprite;
        }else if (currentSong.songID == 002)
        {
            combatAnimationManager.setEnemyAnimator(2);
            enemySpriteRenderer.sprite = redSpiritSprite;
        }
        else if (currentSong.songID == 003)
        {
            combatAnimationManager.setEnemyAnimator(3);
            enemySpriteRenderer.sprite = blueSpiritSprite;
        }
        else if (currentSong.songID == 004)
        {
            combatAnimationManager.setEnemyAnimator(4);
            enemySpriteRenderer.sprite = yellowSpiritSprite;
        }
        else if (currentSong.songID == 005)
        {
            combatAnimationManager.setEnemyAnimator(5);
            enemySpriteRenderer.sprite = aldricSprite;
        }

        
    }

    public void instantWin()
    {
        gameState = 98;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P)) // Checks if "P" key is pressed
        {
            instantWin();
        }
        //handles victory condition
        if (gameState == 98)
        {
            songManager.stopSong();
            if (currentSong.songID != 000)
            {
                VictoryScreen.SetActive(true);

                if (currentSong.songID == 001)
                {
                    gameManager.hasCompletedCombatTut = true;
                }
                else if (currentSong.songID == 002)
                {
                    gameManager.hasCompletedCombat1 = true;
                }
                else if (currentSong.songID == 003)
                {
                    gameManager.hasCompletedCombat2 = true;
                }
                else if (currentSong.songID == 004)
                {
                    gameManager.hasCompletedCombat3 = true;
                }
                else if (currentSong.songID == 005)
                {
                    gameManager.hasCompletedFinal = true;
                }
            }
            else if (currentSong.songID == 000)
            {
                beatManager.hideAllTutorialMessage();
                StartScreen.SetActive(true);
                gameState = 0;
            }
            modeText.text = "YOU WIN";

        }//defeat
        else if (gameState == 99)
        {
            songManager.stopSong();
            if (currentSong.songID == 000)
            {
                StartScreen.SetActive(true);
                gameState = 0;
            }
            else
            {
                DefeatScreen.SetActive(true);
                gameState = 0;
            }
        }
        //otherwise
        else if (gameState != 98 && gameState != 99 && beatManager.songStarted)
        {
            double currentTime = AudioSettings.dspTime;

            if (currentTime > lastCheckedTime) // Ensure we only check once per frame
            {
                lastCheckedTime = currentTime;
                CheckModeSwitch(currentTime);
            }
        }
        
    }


    //select the song based on gamamanager id
    void selectSong()
    {
        //load proper song
        if (gameManager.currentSong == 001)
        {
            currentSong = new ThiefSong();
        }
        else if (gameManager.currentSong == 002)
        {
            currentSong = new Wing1Song();
        }
        else if (gameManager.currentSong == 003)
        {
            currentSong = new Wing2Song();
        }
        else if (gameManager.currentSong == 004)
        {
            currentSong = new Wing3Song();
        }
        else if (gameManager.currentSong == 005)
        {
            currentSong = new AldricSong();
        }

        currentSong.songID = gameManager.currentSong;  

       //currentSong = new ThiefSong();
    }

    void CheckModeSwitch(double currentTime)
    {
        // Check if it's time to switch to attack mode
        if (currentSong.attackModeBeats.Count > 0)
        {
            double attackModeTime = beatManager.GetDspTimeForBeat(currentSong.attackModeBeats[0]);
            if (currentTime >= attackModeTime)
            {
                gameState = 1; // Attack mode
                audioManager.playChangeTurnSound();
                StartCoroutine(ShowTurnBannerWithDelay(0));
                attackBar.SetActive(true);
                Debug.Log("Switched to Attack Mode!");
                currentSong.attackModeBeats.RemoveAt(0); // Remove processed beat
            }
        }

        // Check if it's time to switch to defend mode
        if (currentSong.defendModeBeats.Count > 0)
        {
            double defendModeTime = beatManager.GetDspTimeForBeat(currentSong.defendModeBeats[0]);
            if (currentTime >= defendModeTime)
            {
                gameState = 2; // Defend mode
                audioManager.playChangeTurnSound();
                StartCoroutine(ShowTurnBannerWithDelay(1));
                attackBar.SetActive(false);
                Debug.Log("Switched to Defend Mode!");
                currentSong.defendModeBeats.RemoveAt(0); // Remove processed beat
            }
        }
    }

    private IEnumerator ShowTurnBannerWithDelay(int turn)
    {
        //soundManager.playBannerSound();
        if (turn == 0)
        {
           attackModeBanner.SetActive(true);
        }
        else if (turn == 1)
        {
            defendModeBanner.SetActive(true);
        }

        // banner disappears after a while
        yield return new WaitForSeconds(0.9f);
        attackModeBanner.SetActive(false);
        defendModeBanner.SetActive(false);
    }

    public void OnSkipTutorialClick()
    {
        selectSong();
        beatManager.startSong();
        StartScreen.SetActive(false);

    }
    public void OnStartTutorialClick()
    {
        currentSong = new TestSong();
        beatManager.startSong();
        StartScreen.SetActive(false);

    }

    public void OnContinueButtonClick()
    {
        // Load the UnfinishedSymphony scene
        //sceneController.ExitCombatScene();
    }

    public void OnTryAgainClick()
    {
        selectSong();
        beatManager.startSong();
        DefeatScreen.SetActive(false);
    }
}
