using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    private GameManager gameManager;
    public PlayerController player;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI dialogueText;
    public Image characterPortrait;
    public Sprite playerPortrait;
    private Sprite npcPortrait;
    private NPC currentNPC;
    private Door currentDoor;

    public Animator animator;

    private Queue<string> sentences;
    private Queue<bool> isPlayerSpeakingQueue;
    private string npcName;
    private bool isTyping = false;
    private string currentSentence;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
    
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        player = FindObjectOfType<PlayerController>();
        sentences = new Queue<string>();
        isPlayerSpeakingQueue = new Queue<bool>();
    }

    public void StartDialogue(Dialogue dialogue, Sprite npcPortraitSprite, NPC npc)
    {
        Debug.Log("NPC Portrait Assigned: " + npcPortraitSprite.name);
        currentNPC = npc;
        gameManager.SetGameState(GameManager.GameState.CutScene);

        animator.SetBool("isOpen", true);
        nameText.text = dialogue.npcName;
        npcName = dialogue.npcName;
        npcPortrait = npcPortraitSprite;

        sentences.Clear();
        isPlayerSpeakingQueue.Clear();

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
            return;
        }

        if (sentences.Count == 0)
        {
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
        foreach(char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(0.008f);
        }
        isTyping = false;
    }

    void EndDialogue()
    {
        animator.SetBool("isOpen", false);
        gameManager.SetGameState(GameManager.GameState.Game);

        if (currentNPC != null)
        {
            currentNPC.CompleteInteraction();
        }
    }

    public void StartDoorDialogue(Dialogue dialogue, Sprite npcPortraitSprite, Door door)
    {
        Debug.Log("NPC Portrait Assigned: " + npcPortraitSprite.name);
        currentDoor = door;
        gameManager.SetGameState(GameManager.GameState.CutScene);

        animator.SetBool("isOpen", true);
        nameText.text = dialogue.npcName;
        npcName = dialogue.npcName;
        npcPortrait = npcPortraitSprite;

        sentences.Clear();
        isPlayerSpeakingQueue.Clear();

        for (int i = 0; i < dialogue.sentences.Length; i++)
        {
            sentences.Enqueue(dialogue.sentences[i]);
            isPlayerSpeakingQueue.Enqueue(dialogue.isPlayerSpeaking[i]);
        }
        DisplayNextSentence();
    }
}