using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems; 
using TMPro;

public class PuzzleMechanism : MonoBehaviour
{
    public AudioSource audioSource;          // Main AudioSource for playing the music and notes
    public AudioClip[] noteClips;            // Audio clips for each note
    public Button playButton;                // Button to start the music segment
    public Button[] noteButtons;             // Buttons for playing individual notes
    public Button exitButton;                // Button to exit
    public Image[] missingNoteImages;        // Image components on the missing note buttons
    public Sprite[] noteImages;              // Sprites for each note to display on missing buttons
    public TMP_Text timerText;               // TextMeshProUGUI for displaying the timer
    public TMP_Text feedbackText;            // TextMeshProUGUI for displaying feedback

    public PuzzleLevelConfig levelConfig;    // Level-specific settings
    public PuzzleLevelConfig tutorialConfig;
    public PuzzleLevelConfig levelOneConfig;
    public PuzzleLevelConfig levelTwoConfig;
    public PuzzleLevelConfig levelThreeConfig;
    private GameManager gameManager;
    public Image background1;
    public Image background2;

    private float countdown;                 // Timer countdown
    private bool timerActive = true;         // Flag to control whether the timer should run
    private int[] playerSequence;            // Stores player's sequence of entered notes
    private int attemptCount = 0;            // Number of attempts made

    private Coroutine[] notePreviewCoroutines;
    private SceneController sceneController;
    public Sprite defaultMissingNoteSprite;
    public Button toggleBackgroundButton;

    void Start()
    {
        notePreviewCoroutines = new Coroutine[noteButtons.Length];
        playButton.onClick.AddListener(PlayMusicSegment);
        exitButton.gameObject.SetActive(false); // Hide the exit button initially
        exitButton.onClick.AddListener(() => sceneController.ExitPuzzleScene());
        
        // Load level configuration
        LoadLevelConfig();

        SetupNoteButtons();
        UpdateTimerText(countdown);
        feedbackText.text = "";

        toggleBackgroundButton.onClick.AddListener(ToggleBackgrounds);

        sceneController = FindObjectOfType<SceneController>();

        // Stop background music when entering the puzzle scene
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
    // Set the visibility of background images based on the level
    bool isTutorialLevel = levelConfig == tutorialConfig;
    background1.gameObject.SetActive(isTutorialLevel);
    background2.gameObject.SetActive(isTutorialLevel);

    countdown = levelConfig.timeLimit;
    playerSequence = new int[levelConfig.missingNotesCount];

    // Disable all missing note images first
    foreach (var image in missingNoteImages)
    {
        image.gameObject.SetActive(false);
    }

    // Enable images based on the missingNotesCount from levelConfig
    for (int i = 0; i < levelConfig.missingNotesCount; i++)
    {
        missingNoteImages[i].gameObject.SetActive(true);
    }

    // Hide additional missing note images based on the missingNotesCount
    if (levelConfig.missingNotesCount <= 4)
    {
        // Hide fifth and sixth images if present
        if (missingNoteImages.Length > 4) missingNoteImages[4].gameObject.SetActive(false);
        if (missingNoteImages.Length > 5) missingNoteImages[5].gameObject.SetActive(false);
    }
    else if (levelConfig.missingNotesCount == 5)
    {
        // Hide sixth image if present
        if (missingNoteImages.Length > 5) missingNoteImages[5].gameObject.SetActive(false);
    }
    // If missingNotesCount is 6, all should be visible, no need to hide any

    audioSource.clip = levelConfig.musicSegment;
}





    private void SetupNoteButtons()
    {
        for (int i = 0; i < noteButtons.Length; i++)
        {
            int index = i;
            noteButtons[index].onClick.AddListener(() => HandleNotePress(index));

            EventTrigger trigger = noteButtons[index].gameObject.GetComponent<EventTrigger>() ?? noteButtons[index].gameObject.AddComponent<EventTrigger>();

            // Setup hover enter delay
            EventTrigger.Entry entryEnter = new EventTrigger.Entry { eventID = EventTriggerType.PointerEnter };
            entryEnter.callback.AddListener((data) => StartNotePreviewCoroutine(index, 1.0f));
            trigger.triggers.Add(entryEnter);

            // Setup hover exit to cancel delay
            EventTrigger.Entry entryExit = new EventTrigger.Entry { eventID = EventTriggerType.PointerExit };
            entryExit.callback.AddListener((data) => StopNotePreviewCoroutine(index));
            trigger.triggers.Add(entryExit);
        }
    }

    private void StartNotePreviewCoroutine(int index, float delay)
    {
        StopNotePreviewCoroutine(index);
        notePreviewCoroutines[index] = StartCoroutine(PlayNotePreviewDelayed(index, delay));
    }

    private void StopNotePreviewCoroutine(int index)
    {
        if (notePreviewCoroutines[index] != null)
        {
            StopCoroutine(notePreviewCoroutines[index]);
            notePreviewCoroutines[index] = null;
        }
    }

    IEnumerator PlayNotePreviewDelayed(int index, float delay)
    {
        yield return new WaitForSeconds(delay);
        audioSource.PlayOneShot(noteClips[index]);
    }

    private void HandleNotePress(int index)
    {
        if (attemptCount < levelConfig.missingNotesCount) 
        {
            playerSequence[attemptCount] = index;
            UpdateMissingNoteDisplay(index, attemptCount);
            attemptCount++;

            if (attemptCount == levelConfig.missingNotesCount)
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
        for (int i = 0; i < levelConfig.correctSequence.Length; i++)
        {
            if (playerSequence[i] != levelConfig.correctSequence[i])
            {
                feedbackText.text = "Incorrect sequence, try again!";
                ResetSequence();
                return;
            }
        }

        feedbackText.text = "Victory! +1 missing part of symphony";
        if (tutorialConfig == gameManager.currentPuzzleLevelConfig)
        {
            gameManager.hasCompletedPuzzleTut = true;
            Debug.Log("Updated complete puzzle tut.");
        }
        else if (levelOneConfig == gameManager.currentPuzzleLevelConfig)
        {
            gameManager.hasCompletedPuzzle1 = true;
            Debug.Log("Updated complete puzzle 1.");
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
        PauseTimer();
        ShowExitButton();
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
