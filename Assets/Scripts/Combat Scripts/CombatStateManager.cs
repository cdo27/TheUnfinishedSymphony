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

    public int gameState = 0; // 0 = not started, 1 = attack mode, 2 = defend mode, 98 = victory, 99 = defeat, 198 = victory screen, 199 = defeat screen
    public double lastCheckedTime = -1.0; // Track last checked DSP time

    public SceneController sceneController;
    public GameManager gameManager;
    public PlayerManager playerManager;
    public BeatManager beatManager;
    public AdvantageBarManager advantageBarManager;
    public CombatItemManager combatItemManager;
    public CombatAnimationManager combatAnimationManager;
    public SongManager songManager;
    public AudioManager audioManager;

    public GameObject attackBar;
    public GameObject advantageBar;

    public GameObject attackModeBanner;
    public GameObject defendModeBanner;

    public GameObject StartScreen;
    public GameObject StartButton;
    public TMP_Text enemyNameText;
    public TMP_Text songNameText;
    public TMP_Text BPMText;
    public TMP_Text songLengthText;

    //result screen UI text
    public GameObject ResultScreen;
    public GameObject ResultContinueButton;
    public GameObject ResultTryAgainButton;
    public TMP_Text resultTitle;
    public TMP_Text resultMessage;
    public TMP_Text perfectHitCountText;
    public TMP_Text nearMissCountText;
    public TMP_Text attackWrongKeyText;
    public TMP_Text failedHitText;
    public TMP_Text perfectGuardCountText;
    public TMP_Text weakGuardCountText;
    public TMP_Text defendWrongKeyText;
    public TMP_Text failedBlockText;
    public TMP_Text resultButtonText;

    public GameObject player;
    public GameObject enemy;
    //the enemy hit point 
    public GameObject enemyHitPoint;

    public Sprite thiefSprite;
    public Sprite redSpiritSprite;
    public Sprite yellowSpiritSprite;
    public Sprite blueSpiritSprite;
    public Sprite aldricSprite;
    public Sprite benedictSprite;
    public Sprite escapeSprite;

    // Just for testing
    public TMP_Text modeText;

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();

        playerManager = FindObjectOfType<PlayerManager>();
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

        //set up items
        combatItemManager.itemInitilize();


        //set up the proper song
        selectSong();
        StartScreen.SetActive(true);

        //set up intro screen
        songNameText.text = currentSong.songName;
        enemyNameText.text = currentSong.enemyName;
        songLengthText.text = currentSong.songLength.ToString() + " seconds";
        BPMText.text = currentSong.BPM.ToString();


        //set up the sprites and animators for enemies
        //Animator enemyAnimator = enemy.GetComponent<Animator>();
        SpriteRenderer enemySpriteRenderer = enemy.GetComponent<SpriteRenderer>();
        if (currentSong.songID == 001)
        {
            combatAnimationManager.setEnemyAnimator(1);
            enemySpriteRenderer.sprite = thiefSprite;
        } else if (currentSong.songID == 002)
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
        else if (currentSong.songID == 006)
        {
            combatAnimationManager.setEnemyAnimator(6);
            enemySpriteRenderer.sprite = benedictSprite;
        }
        else if (currentSong.songID == 007)
        {
            combatAnimationManager.setEnemyAnimator(5);
            enemySpriteRenderer.sprite = aldricSprite;
        }

    }

    public void instantWin()
    {
        gameState = 98;  
    }
    public void instantLose()
    {
        gameState = 99;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P)) // Checks if "P" key is pressed
        {
            instantWin();
        }
        if (Input.GetKeyDown(KeyCode.O)) // Checks if "P" key is pressed
        {
            instantLose();
        }
        //handles victory condition
        if (gameState == 98)
        {
            attackBar.SetActive(false);
            advantageBar.SetActive(false);
            //songManager.stopSong();
            if (currentSong.songID != 000)
            {
                gameState = 198;
                songManager.playVictorySong();
                displayResultScreen();

            }
            else if (currentSong.songID == 000)
            {
                songManager.stopSong();
                beatManager.hideAllTutorialMessage();
                StartScreen.SetActive(true);
                gameState = 0;
            }
            modeText.text = "YOU WIN";

        }//defeat
        else if (gameState == 99)
        {
            attackBar.SetActive(false);
            advantageBar.SetActive(false);
            if (currentSong.songID == 000)
            {
                songManager.stopSong();
                beatManager.hideAllTutorialMessage();
                StartScreen.SetActive(true);
                gameState = 0;
            }
            else
            {
                gameState = 199;
                songManager.playDefeatSong();
                displayResultScreen();
            }
        }
        //otherwise
        else if (gameState != 98 && gameState != 99 && gameState != 198 && gameState != 199 && beatManager.songStarted)
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
        else if (gameManager.currentSong == 006)
        {
            currentSong = new BenedictSong();
        }
        else if (gameManager.currentSong == 007)
        {
            currentSong = new EscapeSong();
        }

        currentSong.songID = gameManager.currentSong;
        //currentSong = new BenedictSong();
    }

    public void initilizeAdvantageBar() {
        //calculate number of attack/defend notes for advantage bar scaling
        // Get total number of attack notes
        int totalAttackNotes = currentSong.attackBeatsToHit.Count;
        Debug.Log("attack notes:  " + totalAttackNotes);

        // Get total number of defense notes
        int totalDefenseNotes = 0;
        foreach (var phase in currentSong.defendBeatsToHit)
        {
            totalDefenseNotes += phase.Count;
        }

        advantageBarManager.InitializeBar(totalAttackNotes, totalDefenseNotes);
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
                beatManager.HideActiveShield();
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
        if (gameState == 0)
        {
            selectSong();
            beatManager.startSong();
            StartScreen.SetActive(false);
            advantageBar.SetActive(true);
        }
    }
    public void OnStartTutorialClick()
    {
        currentSong = new TestSong();
        beatManager.startSong();
        StartScreen.SetActive(false);
        advantageBar.SetActive(true);

    }

    //for result screen
    public void displayResultScreen()
    {
        //result title, message, and button text
        if (gameState == 198)
        {
            resultTitle.text = "victory";
            resultMessage.text = "Symphony Sheet Obtained";
            if (currentSong.songID == 005 || currentSong.songID == 006 || currentSong.songID == 007)
            {
                resultMessage.text = "You have overcome the final obstacle!";
            }
            ResultContinueButton.SetActive(true);
        }
        else if (gameState == 199)
        {
            resultTitle.text = "defeat";
            resultMessage.text = "Failed to Obtain Symphony Sheet";
            ResultTryAgainButton.SetActive(true);
        }

        //attack phase result
        perfectHitCountText.text = beatManager.attackResults[0].ToString();
        nearMissCountText.text = beatManager.attackResults[1].ToString();
        attackWrongKeyText.text = beatManager.attackResults[2].ToString();
        failedHitText.text = beatManager.attackResults[3].ToString();

        perfectGuardCountText.text = beatManager.defendResults[0].ToString();
        weakGuardCountText.text = beatManager.defendResults[1].ToString();
        defendWrongKeyText.text = beatManager.defendResults[2].ToString();
        failedBlockText.text = beatManager.defendResults[3].ToString();

        ResultScreen.SetActive(true);
    }


    //result button handling

    public void OnContinueButtonClick()
    {
        songManager.stopSong();
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
        else if (currentSong.songID == 005 || currentSong.songID == 005 ||  currentSong.songID == 005)
        {
            gameManager.hasCompletedFinal = true;
        }
    }

    public void OnTryAgainClick()
    {
        songManager.stopSong();
        gameState = 0;
        selectSong();
        beatManager.startSong();
        advantageBar.SetActive(true);
        ResultTryAgainButton.SetActive(false);
        ResultScreen.SetActive(false);
    }
}
