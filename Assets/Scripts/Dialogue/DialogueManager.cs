using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    private GameManager gameManager;
    private AudioManager audioManager;

    public PlayerController player;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI dialogueText;
    public Image characterPortrait;
    public Sprite playerPortrait;
    private Sprite npcPortrait;
    private NPC currentNPC;
    private Door currentDoor;

    public Animator animator;
    public GameObject choicePanel;
    public Button choice1Button;
    public Button choice2Button;
    public TextMeshProUGUI choice1Text;
    public TextMeshProUGUI choice2Text;

    private Queue<string> sentences;
    private Queue<bool> isPlayerSpeakingQueue;
    private string npcName;
    private bool isTyping = false;
    private string currentSentence;
    private bool isChoiceActive = false;
    private ChoiceDialogue currentChoiceDialogue;
    private bool choiceMade = false;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
    
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        audioManager = FindObjectOfType<AudioManager>();
        player = FindObjectOfType<PlayerController>();
        sentences = new Queue<string>();
        isPlayerSpeakingQueue = new Queue<bool>();
        choicePanel.SetActive(false);
    }

    public void StartDialogue(Dialogue dialogue, Sprite npcPortraitSprite, NPC npc)
    {
        Debug.Log("Starting dialogue for: " + dialogue.npcName);
        currentNPC = npc;
        currentChoiceDialogue = dialogue as ChoiceDialogue;
        gameManager.SetGameState(GameManager.GameState.CutScene);

        animator.SetBool("isOpen", true);
        nameText.text = dialogue.npcName;
        npcName = dialogue.npcName;
        npcPortrait = npcPortraitSprite;

        sentences.Clear();
        isPlayerSpeakingQueue.Clear();
        choiceMade = false;

        for (int i = 0; i < dialogue.sentences.Length; i++)
        {
            sentences.Enqueue(dialogue.sentences[i]);
            isPlayerSpeakingQueue.Enqueue(dialogue.isPlayerSpeaking[i]);
        }
        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if (isTyping)
        {
            StopAllCoroutines();
            dialogueText.text = currentSentence;
            isTyping = false;
            Debug.Log("Finished typing current sentence");
            return;
        }

        if (sentences.Count == 0)
        {
            if (currentChoiceDialogue != null && 
                !string.IsNullOrEmpty(currentChoiceDialogue.choice1) && 
                !string.IsNullOrEmpty(currentChoiceDialogue.choice2) && 
                !isChoiceActive && !choiceMade)
            {
                Debug.Log("Displaying choices");
                DisplayChoices();
                return;
            }
            Debug.Log("Ending dialogue");
            EndDialogue();
            return;
        }

        currentSentence = sentences.Dequeue();
        bool isPlayerSpeaking = isPlayerSpeakingQueue.Dequeue();

        if (isPlayerSpeaking)
        {
            nameText.text = "Lucien";
            characterPortrait.sprite = playerPortrait;
        }
        else
        {
            nameText.text = npcName;
            characterPortrait.sprite = npcPortrait;
        }

        StopAllCoroutines();
        StartCoroutine(TypeSentence(currentSentence));
    }

    IEnumerator TypeSentence(string sentence)
    {
        isTyping = true;
        dialogueText.text = "";
        audioManager?.PlayDialogueSFX();
        foreach(char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(0.008f);
        }
        isTyping = false;
        audioManager?.StopDialogueSFX();
    }

    void DisplayChoices()
    {
        isChoiceActive = true;
        choicePanel.SetActive(true);
        choice1Text.text = currentChoiceDialogue.choice1;
        choice2Text.text = currentChoiceDialogue.choice2;

        choice1Button.interactable = true;
        choice2Button.interactable = true;

        choice1Button.onClick.RemoveAllListeners();
        choice2Button.onClick.RemoveAllListeners();

        choice1Button.onClick.AddListener(() => SelectChoice(0));
        choice2Button.onClick.AddListener(() => SelectChoice(1));

        Debug.Log("Choices displayed.");
    }

    void SelectChoice(int choiceIndex)
    {
        
        choicePanel.SetActive(false);
        isChoiceActive = false;
        choiceMade = true;

        //update gamemanager which choice they made
        if(choiceIndex == 0){
            gameManager.Ending1 = true;
            gameManager.hasMadeChoice = true;
            Debug.Log("Choice 1 selected");
        }else if(choiceIndex == 1){
            gameManager.Ending2 = true;
            gameManager.hasMadeChoice = true;
            Debug.Log("Choice 2 selected");
        }

        if (currentChoiceDialogue != null)
        {
            sentences.Clear();
            isPlayerSpeakingQueue.Clear();

            string[] outcomes = (choiceIndex == 0) ? currentChoiceDialogue.outcome1 : currentChoiceDialogue.outcome2;
            if (outcomes != null && outcomes.Length > 0)
            {
                foreach (string sentence in outcomes)
                {
                    sentences.Enqueue(sentence);
                    isPlayerSpeakingQueue.Enqueue(false); 
                    Debug.Log("Queued outcome: " + sentence);
                }
            }
            else
            {
                Debug.LogWarning("No outcomes defined for choice " + choiceIndex);
                EndDialogue();
                return;
            }
        }
        else
        {
            Debug.LogWarning("CurrentChoiceDialogue is null");
            EndDialogue();
            return;
        }

        DisplayNextSentence();
    }

    void EndDialogue()
    {
        animator.SetBool("isOpen", false);
        choicePanel.SetActive(false);
        isChoiceActive = false;
        gameManager.SetGameState(GameManager.GameState.Game);

        if (currentNPC != null)
        {
            currentNPC.CompleteInteraction();
        }
        Debug.Log("Dialogue ended");
        audioManager?.StopDialogueSFX();
    }

    public void StartDoorDialogue(Dialogue dialogue, Sprite npcPortraitSprite, Door door)
    {
        Debug.Log("Starting door dialogue for: " + dialogue.npcName);
        currentDoor = door;
        currentChoiceDialogue = dialogue as ChoiceDialogue;
        gameManager.SetGameState(GameManager.GameState.CutScene);

        animator.SetBool("isOpen", true);
        nameText.text = dialogue.npcName;
        npcName = dialogue.npcName;
        npcPortrait = npcPortraitSprite;

        sentences.Clear();
        isPlayerSpeakingQueue.Clear();
        choiceMade = false;

        for (int i = 0; i < dialogue.sentences.Length; i++)
        {
            sentences.Enqueue(dialogue.sentences[i]);
            isPlayerSpeakingQueue.Enqueue(dialogue.isPlayerSpeaking[i]);
        }
        DisplayNextSentence();
    }
}