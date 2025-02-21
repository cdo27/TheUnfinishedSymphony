using UnityEngine;
using UnityEngine.UI;
using TMPro; // Add this namespace for TextMeshPro
using System.Collections;

public class ScrollingTextUI : MonoBehaviour
{
    [Header("UI Elements")]
    public TMP_Text displayText; 
    public GameObject continueButton;  
    public RectTransform textRect;      

    [Header("Settings")]
    public float typingSpeed = 0.05f; 
    public float delayBeforeTyping = 0.5f; // Delay before typing starts
    [TextArea(3, 10)]                    
    public string defaultText = "Default typewriter text..."; 

    private bool isTyping = false;
    private string fullText = "";       

    void Awake()
    {
        // Ensure UI elements are assigned
        if (displayText == null)
            displayText = GetComponent<TMP_Text>();
        if (continueButton == null)
            Debug.LogError("Continue Button not assigned in TypewriterTextUI!");

        if (continueButton != null)
            continueButton.SetActive(false);


        if (displayText != null)
            displayText.text = "";
    }

    public void StartTypewriterText(string text = "")
    {
        if (isTyping) return; 

        fullText = string.IsNullOrEmpty(text) ? defaultText : text;
        if (displayText != null)
            displayText.text = "";

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
                displayText.text = fullText.Substring(0, i);
            yield return new WaitForSeconds(typingSpeed); // Wait between letters
        }

        if (continueButton != null)
            continueButton.SetActive(true);

        isTyping = false;
    }

    public void OnContinueClicked()
    {
        Debug.Log("Continue button clicked!");
   
        if (continueButton != null)
            continueButton.SetActive(false);
    }


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