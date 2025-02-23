using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;

public class PuzzleMechanism : MonoBehaviour
{
    public AudioSource audioSource;          // Main AudioSource for playing the music and notes
    public AudioClip[] noteClips;            // Audio clips for each note
    public Button playButton;                // Button to start the music segment
    public Button[] noteButtons;             // Buttons for playing individual notes
    public Button[] newNoteButtons;          // New buttons for playing additional notes
    public Button exitButton;                // Button to exit
    public Image[] missingNoteImages;        // Image components on the missing note buttons
    public Sprite[] noteImages;              // Sprites for each note to display on missing buttons
    public TMP_Text timerText;               // TextMeshProUGUI for displaying the timer
    public TMP_Text feedbackText;            // TextMeshProUGUI for displaying feedback

    public PuzzleLevelConfig levelConfig;    
    public PuzzleLevelConfig tutorialConfig;
    public PuzzleLevelConfig levelOneConfig;
    public PuzzleLevelConfig levelTwoConfig;
    public PuzzleLevelConfig levelThreeConfig;
    
    private GameManager gameManager;
    public Image background1;
    public Image background2;

    private float countdown;
    private bool timerActive = true;
    private int[] playerSequence;
    private int attemptCount = 0;

    private SceneController sceneController;
    public Sprite defaultMissingNoteSprite;
    public Button toggleBackgroundButton;
    private int selectedMissingNoteIndex = -1;

    void Start()
    {
        playButton.onClick.AddListener(PlayMusicSegment);
        exitButton.gameObject.SetActive(false);
        exitButton.onClick.AddListener(() => sceneController.ExitPuzzleScene());

        LoadLevelConfig();
        SetupNoteButtons();
        SetupNewNoteButtons();
        SetupMissingNoteImages();
        UpdateTimerText(countdown);
        feedbackText.text = "";

        toggleBackgroundButton.onClick.AddListener(ToggleBackgrounds);
        sceneController = FindObjectOfType<SceneController>();

        if (gameManager != null && gameManager.audioManager != null)
        {
            gameManager.audioManager.StopBackgroundMusic();
        }
    }

    void Update()
    {
        if (timerActive && countdown > 0)
        {
            countdown -= Time.deltaTime;
            UpdateTimerText(countdown);
        }
        else if (timerActive && countdown <= 0)
        {
            countdown = 0;
            UpdateTimerText(countdown);
            TimerEnded();
        }
    }

    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();

        if (gameManager == null)
        {
            Debug.LogError("GameManager not found in PuzzleMechanism!");
            return;
        }

        if (gameManager.currentPuzzleLevelConfig != null)
        {
            SetLevelConfig(gameManager.currentPuzzleLevelConfig);
        }
        else
        {
            Debug.LogError("No level config found in GameManager when loading puzzle.");
        }
    }
    private void SetupNewNoteButtons()
    {
        if (newNoteButtons.Length != noteClips.Length)
        {
            Debug.LogError("The number of new note buttons does not match the number of note clips.");
            return;
        }

        for (int i = 0; i < newNoteButtons.Length; i++)
        {
            int clipIndex = i;
            newNoteButtons[i].onClick.AddListener(() => PlayNoteClip(clipIndex));
        }
    }

    private void PlayNoteClip(int index)
    {
        if (index >= 0 && index < noteClips.Length)
        {
            audioSource.PlayOneShot(noteClips[index]);
        }
    }

    private void SetupMissingNoteImages()
    {
        for (int i = 0; i < missingNoteImages.Length; i++)
        {
            int index = i;
            missingNoteImages[i].GetComponent<Button>().onClick.AddListener(() => SelectMissingNoteImage(index));
            missingNoteImages[i].gameObject.SetActive(i < levelConfig.missingNotesCount);
        }
    }

    private void SelectMissingNoteImage(int index)
    {
        selectedMissingNoteIndex = index;
    }

    private void ToggleBackgrounds()
    {
        bool isActive = background1.gameObject.activeSelf;
        background1.gameObject.SetActive(!isActive);
        background2.gameObject.SetActive(!isActive);
    }

    public void SetLevelConfig(PuzzleLevelConfig config)
    {
        levelConfig = config;
        LoadLevelConfig();
        ResetSequence();
        UpdateTimerText(countdown);
        feedbackText.text = "Start playing!";
    }

    private void LoadLevelConfig()
    {
        if (levelConfig == null)
        {
            Debug.LogError("Level configuration not set!");
            return;
        }

        bool isTutorialLevel = levelConfig == tutorialConfig;
        background1.gameObject.SetActive(isTutorialLevel);
        background2.gameObject.SetActive(isTutorialLevel);

        countdown = levelConfig.timeLimit;
        playerSequence = new int[levelConfig.missingNotesCount];

        foreach (var image in missingNoteImages)
        {
            image.gameObject.SetActive(false);
        }

        for (int i = 0; i < levelConfig.missingNotesCount; i++)
        {
            missingNoteImages[i].gameObject.SetActive(true);
        }

        if (levelConfig.missingNotesCount <= 3)
        {
            if (missingNoteImages.Length > 4) missingNoteImages[4].gameObject.SetActive(false);
            if (missingNoteImages.Length > 5) missingNoteImages[5].gameObject.SetActive(false);
            if (missingNoteImages.Length > 6) missingNoteImages[6].gameObject.SetActive(false);
        }
        else if (levelConfig.missingNotesCount == 4)
        {   
            if (missingNoteImages.Length > 4) missingNoteImages[4].gameObject.SetActive(false);
            if (missingNoteImages.Length > 5) missingNoteImages[5].gameObject.SetActive(false);
        }
        else if (levelConfig.missingNotesCount == 5)
        {
            if (missingNoteImages.Length > 5) missingNoteImages[5].gameObject.SetActive(false);
        }

        audioSource.clip = levelConfig.musicSegment;
    }

    private void SetupNoteButtons()
    {
        for (int i = 0; i < noteButtons.Length; i++)
        {
            int index = i;
            noteButtons[index].onClick.AddListener(() => HandleNotePress(index));
        }
    }

    private void HandleNotePress(int index)
    {
        if (selectedMissingNoteIndex != -1)
        {
            if (index < noteImages.Length)
            {
                missingNoteImages[selectedMissingNoteIndex].sprite = noteImages[index];
                playerSequence[selectedMissingNoteIndex] = index;
                if (++attemptCount == levelConfig.missingNotesCount)
                {
                    CheckSequence();
                }
            }
            selectedMissingNoteIndex = -1;
        }
        else if (attemptCount < levelConfig.missingNotesCount)
        {
            playerSequence[attemptCount] = index;
            UpdateMissingNoteDisplay(index, attemptCount);
            if (++attemptCount == levelConfig.missingNotesCount)
            {
                CheckSequence();
            }
        }
    }

    private void UpdateMissingNoteDisplay(int noteIndex, int missingIndex)
    {
        if (noteIndex < noteImages.Length)
        {
            missingNoteImages[missingIndex].sprite = noteImages[noteIndex];
        }
    }

    private void CheckSequence()
    {
        bool isCorrect = true;
        for (int i = 0; i < levelConfig.correctSequence.Length; i++)
        {
            if (playerSequence[i] != levelConfig.correctSequence[i])
            {
                isCorrect = false;
                break;
            }
        }

        if (isCorrect)
        {
            feedbackText.text = "Finish the Puzzle!";
            PauseTimer();
            ShowExitButton();

            if (tutorialConfig == gameManager.currentPuzzleLevelConfig)
            {
                gameManager.hasCompletedPuzzleTut = true;
            }
            else if (levelOneConfig == gameManager.currentPuzzleLevelConfig)
            {
                gameManager.hasCompletedPuzzle1 = true;
            }
            else if (levelTwoConfig == gameManager.currentPuzzleLevelConfig)
            {
                gameManager.hasCompletedPuzzle2 = true;
            }
            else if (levelThreeConfig == gameManager.currentPuzzleLevelConfig)
            {
                gameManager.hasCompletedPuzzle3 = true;
            }
            else
            {
                Debug.Log("Unknown level configuration.");
            }
        }
        else
        {
            feedbackText.text = "Incorrect sequence, try again!";
            ResetSequence();
        }
    }

    private void TimerEnded()
    {
        feedbackText.text = "Defeated! Failed to retrieve missing part";
        PauseTimer();
        ShowExitButton();
    }

    private void ShowExitButton()
    {
        exitButton.gameObject.SetActive(true);
        DisableAllButtons();
    }

    private void ResetSequence()
    {
        attemptCount = 0;
        selectedMissingNoteIndex = -1;

        for (int i = 0; i < missingNoteImages.Length; i++)
        {
            if (missingNoteImages[i] != null)
            {
                missingNoteImages[i].sprite = defaultMissingNoteSprite;
            }
        }
    }

    private void DisableAllButtons()
    {
        foreach (Button btn in noteButtons)
        {
            btn.interactable = false;
        }
    }

    private void UpdateTimerText(float time)
    {
        timerText.text = "Timer: " + time.ToString("F2") + "s";
    }

    private void PauseTimer()
    {
        timerActive = false;
    }

    void PlayMusicSegment()
    {
        if (!audioSource.isPlaying)
        {
            audioSource.clip = levelConfig.musicSegment;
            audioSource.Play();
        }
    }
}
