using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;

public class PuzzleMechanism : MonoBehaviour
{
    public AudioSource audioSource;          // Main AudioSource for playing the music and notes
    public AudioClip[] noteClips;            // Audio clips for each note
    public AudioClip clickSound;   
    public AudioClip dropSound; 
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
    private int selectedNoteButtonIndex = -1;
    private PlayerManager playerManager;
    public Button resetButton;

    void Start()
    {
        playButton.onClick.AddListener(PlayMusicSegment);
        exitButton.gameObject.SetActive(false);
        exitButton.onClick.AddListener(() => sceneController.ExitPuzzleScene());

        LoadLevelConfig();
        SetupNoteButtons();
        SetupNewNoteButtons();
        resetButton.onClick.AddListener(ResetGame);
        SetupMissingNoteImages();
        UpdateTimerText(countdown);
        feedbackText.text = "";

        toggleBackgroundButton.onClick.AddListener(ToggleBackgrounds);
        sceneController = FindObjectOfType<SceneController>();

        if (gameManager != null && gameManager.audioManager != null)
        {
            gameManager.audioManager.StopBackgroundMusic();
        }

        SetupButtonSounds();
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
        playerManager = FindObjectOfType<PlayerManager>();

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
     // Method to set up click sounds
    private void SetupButtonSounds()
    {
        // Apply the click sound to all buttons except those meant for playing music or notes
        foreach (var button in new Button[] { exitButton, resetButton, toggleBackgroundButton }) // Add more buttons as needed
        {
            button.onClick.AddListener(PlayClickSound);
        }

        // Optionally, apply to newNoteButtons if they are not playing notes directly
        foreach (var button in newNoteButtons)
        {
            button.onClick.AddListener(PlayClickSound);
        }
    }

    // Method to play the click sound
    private void PlayClickSound()
    {
        audioSource.PlayOneShot(clickSound);
    }
    private void ResetGame()
    {
        playerSequence = new int[levelConfig.missingNotesCount];
        attemptCount = 0;
        selectedMissingNoteIndex = -1;

        foreach (var image in missingNoteImages)
        {
            if (image != null)
                image.sprite = defaultMissingNoteSprite;
        }

        feedbackText.text = "Puzzle reset. Try again!";

        if (selectedNoteButtonIndex != -1)
        {
            newNoteButtons[selectedNoteButtonIndex].GetComponent<Image>().color = Color.white;
            selectedNoteButtonIndex = -1;
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
            // Play the corresponding note clip
            audioSource.PlayOneShot(noteClips[index]);

            // Visual feedback for selection
            if (selectedNoteButtonIndex != -1)  // Reset the previous selected button if any
            {
                newNoteButtons[selectedNoteButtonIndex].GetComponent<Image>().color = Color.white;
            }

            // Highlight the currently selected button
            newNoteButtons[index].GetComponent<Image>().color = Color.grey;
            selectedNoteButtonIndex = index;  // Update the currently selected button index
        }
    }

    private void SetupMissingNoteImages()
    {
        for (int i = 0; i < missingNoteImages.Length; i++)
        {
            int index = i;
            Button imageButton = missingNoteImages[i].GetComponent<Button>();
            missingNoteImages[i].GetComponent<Button>().onClick.AddListener(() => SelectMissingNoteImage(index));
            missingNoteImages[i].gameObject.SetActive(i < levelConfig.missingNotesCount);
            imageButton.onClick.AddListener(PlayClickSound);
            
            
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
        if (playerManager != null && playerManager.GetOwnedItems().Contains(3))
    {
        countdown += 10f;
    }
        playerSequence = new int[levelConfig.missingNotesCount];
        for (int i = 0; i < levelConfig.missingNotesCount; i++)
    {
        playerSequence[i] = -1;
    }

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
    public void HandleDropNote(int noteIndex, int missingIndex)
    {
        if (noteIndex < 0 || noteIndex >= noteImages.Length)
            return;

        // Only increment attemptCount if this missing note was not previously filled.
        if (playerSequence[missingIndex] == -1 || missingNoteImages[missingIndex].sprite == defaultMissingNoteSprite)
        {
            attemptCount++;
        }

        // Set the missing note image sprite and record the note in the player's sequence.
        missingNoteImages[missingIndex].sprite = noteImages[noteIndex];
        playerSequence[missingIndex] = noteIndex;
        
        // <-- Play the drop sound effect when a note is successfully placed.
        if (dropSound != null)
        {
            audioSource.PlayOneShot(dropSound);
        }

        // If all missing notes have been filled, check the sequence.
        if (attemptCount == levelConfig.missingNotesCount)
        {
            if (IsAllInputsMade())
            {
                CheckSequence();
            }
        }
    }


    private void SetupNoteButtons()
    {
        for (int i = 0; i < noteButtons.Length; i++)
        {
            int index = i;
            noteButtons[index].onClick.AddListener(() => HandleNotePress(index));
            noteButtons[index].onClick.AddListener(PlayClickSound);
        }
    }

    private void HandleNotePress(int index)
{
    if (selectedMissingNoteIndex != -1)
    {
        if (index < noteImages.Length)
        {
            // Only increment attempt count if the image was previously unset or the default sprite
            if (playerSequence[selectedMissingNoteIndex] == -1 || missingNoteImages[selectedMissingNoteIndex].sprite == defaultMissingNoteSprite)
            {
                attemptCount++;
            }

            missingNoteImages[selectedMissingNoteIndex].sprite = noteImages[index];
            playerSequence[selectedMissingNoteIndex] = index;
            selectedMissingNoteIndex = -1;

            // After setting, check if all inputs are made to decide if CheckSequence should be called
            if (attemptCount == levelConfig.missingNotesCount)
            {
                if (IsAllInputsMade())
                {
                    CheckSequence();
                }
            }
        }
    }
}


// Helper method to verify if all inputs are correctly made
private bool IsAllInputsMade()
{
    for (int i = 0; i < levelConfig.missingNotesCount; i++)
    {
        // Assuming default or initial value in playerSequence is -1 or another invalid value
        if (playerSequence[i] == -1)  // Check if all elements have been correctly assigned a valid index
        {
            return false;
        }
    }
    return true;
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
            feedbackText.text = "Music Sheet Completed!";
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
