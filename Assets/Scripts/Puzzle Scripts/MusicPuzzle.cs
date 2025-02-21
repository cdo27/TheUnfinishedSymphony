using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems; 
using TMPro;

public class PuzzleMechanism : MonoBehaviour
{
    public AudioSource audioSource;          // Main AudioSource for playing the music and notes
    public AudioClip musicSegment;           // The main music segment
    public AudioClip[] noteClips;            // Audio clips for each note
    public Button playButton;                // Button to start the music segment
    public Button[] noteButtons;             // Buttons for playing individual notes
    public Button exitButton;                // Button to the exit
    public Image[] missingNoteImages;        // Image components on the missing note buttons
    public Sprite[] noteImages;              // Sprites for each note to display on missing buttons
    public TMP_Text timerText;               // TextMeshProUGUI for displaying the timer
    public TMP_Text feedbackText;            // TextMeshProUGUI for displaying feedback

    private float countdown = 30.0f;         // Timer countdown from 30 seconds
    private bool timerActive = true;         // Flag to control whether the timer should run
    private int[] correctSequence = {1, 5, 6, 3}; // Indices for the correct sequence of notes
    private int[] playerSequence = new int[4];    // Array to store player's sequence of note indices
    private int attemptCount = 0;                 // Number of attempts made

    private SceneController sceneController;

    void Start()
    {
        exitButton.gameObject.SetActive(false); // Hide the exit button initially
        exitButton.onClick.AddListener(() => sceneController.ExitPuzzleScene());
        playButton.onClick.AddListener(PlayMusicSegment);
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

    private void SetupNoteButtons()
{
    for (int i = 0; i < noteButtons.Length; i++)
    {
        int index = i;
        noteButtons[index].onClick.AddListener(() => HandleNotePress(index));

        EventTrigger trigger = noteButtons[index].gameObject.GetComponent<EventTrigger>() ?? noteButtons[index].gameObject.AddComponent<EventTrigger>();

        // Setup hover enter delay
        EventTrigger.Entry entryEnter = new EventTrigger.Entry { eventID = EventTriggerType.PointerEnter };
        entryEnter.callback.AddListener((data) => StartCoroutine(PlayNotePreviewDelayed(index, 1.0f))); // 1 second delay
        trigger.triggers.Add(entryEnter);

        // Setup hover exit to cancel delay
        EventTrigger.Entry entryExit = new EventTrigger.Entry { eventID = EventTriggerType.PointerExit };
        entryExit.callback.AddListener((data) => StopCoroutine(PlayNotePreviewDelayed(index, 1.0f)));
        trigger.triggers.Add(entryExit);
    }
}
IEnumerator PlayNotePreviewDelayed(int index, float delay)
{
    yield return new WaitForSeconds(delay);
    audioSource.PlayOneShot(noteClips[index]);
}


private void PlayNotePreview(int index)
{
    audioSource.PlayOneShot(noteClips[index]); // Play the note sound on hover
}


    private void HandleNotePress(int index)
    {
        if (attemptCount < 4) // Only allow interaction if less than 4 notes have been entered
        {
            audioSource.PlayOneShot(noteClips[index]);
            playerSequence[attemptCount] = index;
            UpdateMissingNoteDisplay(index, attemptCount);
            attemptCount++;

            if (attemptCount == 4)
            {
                CheckSequence();
            }
        }
    }

    private void UpdateMissingNoteDisplay(int noteIndex, int missingIndex)
    {
        if (noteIndex < noteImages.Length)
        {
            missingNoteImages[missingIndex].sprite = noteImages[noteIndex]; // Set the sprite to the corresponding note image
        }
    }

    private void CheckSequence()
{
    for (int i = 0; i < correctSequence.Length; i++)
    {
        if (playerSequence[i] != correctSequence[i])
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
    exitButton.gameObject.SetActive(true); // Show the exit button
    DisableAllButtons(); // Optional: Disable other buttons if necessary
}



    private void ResetSequence()
    {
        attemptCount = 0;
        foreach (var image in missingNoteImages)
        {
            image.sprite = null; // Clear all images
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
        timerActive = false;  // Set timerActive to false to stop the countdown
    }

    void PlayMusicSegment()
    {
        if (!audioSource.isPlaying)
        {
            audioSource.clip = musicSegment;
            audioSource.Play();
        }
    }
}
