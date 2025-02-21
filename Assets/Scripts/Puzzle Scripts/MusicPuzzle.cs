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

    private float countdown;                 // Timer countdown
    private bool timerActive = true;         // Flag to control whether the timer should run
    private int[] playerSequence;            // Stores player's sequence of entered notes
    private int attemptCount = 0;            // Number of attempts made

    private Coroutine[] notePreviewCoroutines;
    private SceneController sceneController;

    void Start()
    {
        notePreviewCoroutines = new Coroutine[noteButtons.Length];
        exitButton.gameObject.SetActive(false); // Hide the exit button initially
        exitButton.onClick.AddListener(() => sceneController.ExitPuzzleScene());
        playButton.onClick.AddListener(PlayMusicSegment);
        
        // Load level configuration
        LoadLevelConfig();

        SetupNoteButtons();
        UpdateTimerText(countdown);
        feedbackText.text = "";

        sceneController = FindObjectOfType<SceneController>();
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

    countdown = levelConfig.timeLimit;
    playerSequence = new int[levelConfig.missingNotesCount];

    // Adjust UI for the number of missing notes
    foreach (var image in missingNoteImages)
    {
        image.gameObject.SetActive(false); // Initially disable all
    }

    for (int i = 0; i < levelConfig.missingNotesCount; i++)
    {
        missingNoteImages[i].gameObject.SetActive(true);
    }

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

        feedbackText.text = "Congratulations! You've completed the puzzle.";
        PauseTimer();
        ShowExitButton();
    }

    private void TimerEnded()
    {
        feedbackText.text = "Time's up! Please try again.";
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
        foreach (var image in missingNoteImages)
        {
            image.sprite = null;
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
