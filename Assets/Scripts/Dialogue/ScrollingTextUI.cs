using UnityEngine;
using UnityEngine.UI;
using TMPro; // Add this namespace for TextMeshPro
using System.Collections;

public class ScrollingTextUI : MonoBehaviour
{
    [Header("UI Elements")]
    public TMP_Text displayText;         // TextMeshPro Text component for the typewriter effect
    public GameObject continueButton;    // The Button GameObject to show when text is fully displayed
    public RectTransform textRect;       // RectTransform of the text (optional, for future positioning)

    [Header("Settings")]
    public float typingSpeed = 0.05f;    // Time between each letter (seconds)
    public float delayBeforeTyping = 0.5f; // Delay before typing starts
    [TextArea(3, 10)]                    // Allows multi-line text input in Inspector
    public string defaultText = "Default typewriter text..."; // Default text set in Inspector

    private bool isTyping = false;
    private string fullText = "";        // Full text to display

    void Awake()
    {
        // Ensure UI elements are assigned
        if (displayText == null)
            displayText = GetComponent<TMP_Text>();
        if (continueButton == null)
            Debug.LogError("Continue Button not assigned in TypewriterTextUI!");

        // Hide continue button initially
        if (continueButton != null)
            continueButton.SetActive(false);

        // Set initial text from Inspector
        if (displayText != null)
            displayText.text = ""; // Start with empty text
    }

    // Call this method to start the typewriter effect (e.g., from CutsceneManager or DialogueManager)
    public void StartTypewriterText(string text = "")
    {
        if (isTyping) return; // Prevent multiple typings at once

        // Use provided text or fall back to defaultText from Inspector
        fullText = string.IsNullOrEmpty(text) ? defaultText : text;
        if (displayText != null)
            displayText.text = ""; // Clear current text

        // Hide continue button
        if (continueButton != null)
            continueButton.SetActive(false);

        // Start the typing coroutine
        StartCoroutine(TypeText());
    }

    private IEnumerator TypeText()
    {
        isTyping = true;

        // Wait before typing starts
        yield return new WaitForSeconds(delayBeforeTyping);

        // Type letter by letter
        for (int i = 0; i <= fullText.Length; i++)
        {
            if (displayText != null)
                displayText.text = fullText.Substring(0, i); // Display up to current letter
            yield return new WaitForSeconds(typingSpeed); // Wait between letters
        }

        // Show continue button when typing is complete
        if (continueButton != null)
            continueButton.SetActive(true);

        isTyping = false;
    }

    // Optional: Method to handle continue button click (e.g., to proceed with dialogue or cutscene)
    public void OnContinueClicked()
    {
        Debug.Log("Continue button clicked!");
        // Add logic here, e.g., call CutsceneManager.nextStep(), DialogueManager.NextDialogue(), etc.
        if (continueButton != null)
            continueButton.SetActive(false); // Hide button after clicking
    }

    // Optional: Skip to full text immediately (e.g., for testing or player input)
    public void SkipToFullText()
    {
        if (isTyping && displayText != null)
        {
            displayText.text = fullText;
            StopCoroutine(TypeText());
            isTyping = false;
            if (continueButton != null)
                continueButton.SetActive(true);
        }
    }
}