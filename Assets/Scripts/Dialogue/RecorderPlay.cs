using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RecorderPlay : MonoBehaviour
{
    private GameManager gameManager;
    public PlayerController player;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI dialogueText;
    public Image characterPortrait;
    public Sprite playerPortrait;
    private Sprite npc1Portrait;
    private Sprite npc2Portrait;
    private Sprite npc3Portrait;
    private NPC currentNPC;

    public Animator animator;

    public Queue<string> sentences;
    public Queue<bool> isPlayerSpeakingQueue;
    public Queue<bool> isAldricSpeakingQueue;
    public Queue<bool> isNPC2SpeakingQueue;
    public string npc1Name;
    public string npc2Name;
    public string npc3Name;

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
        isAldricSpeakingQueue = new Queue<bool>();
        isNPC2SpeakingQueue = new Queue<bool>();
    }

    public void StartDialogue(RecorderDialogue recorder, Sprite[] npcPortraits, NPC npc)
    {
        if (npcPortraits.Length < 3)
        {
            Debug.LogError("Need 3 NPC portraits for dialogue!");
            return;
        }

        currentNPC = npc;
        gameManager.SetGameState(GameManager.GameState.CutScene);

        animator.SetBool("isOpen", true);
        
        npc1Name = recorder.npc1Name;
        npc2Name = recorder.npc2Name;
        npc3Name = recorder.npc3Name;
        npc1Portrait = npcPortraits[0];
        npc2Portrait = npcPortraits[1];
        npc3Portrait = npcPortraits[2];

        Debug.Log($"Dialogue started with {npc1Name}, {npc2Name}, and {npc3Name}");

        sentences.Clear();
        isPlayerSpeakingQueue.Clear();
        isAldricSpeakingQueue.Clear();
        isNPC2SpeakingQueue.Clear();

        for (int i = 0; i < recorder.sentences.Length; i++)
        {
            sentences.Enqueue(recorder.sentences[i]);
            isPlayerSpeakingQueue.Enqueue(recorder.isPlayerSpeaking[i]);
            isAldricSpeakingQueue.Enqueue(recorder.isAldricSpeaking[i]);
            isNPC2SpeakingQueue.Enqueue(recorder.isNPC2Speaking[i]);
        }
        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        string sentence = sentences.Dequeue();
        bool isPlayerSpeaking = isPlayerSpeakingQueue.Dequeue();
        bool isAldricSpeaking = isAldricSpeakingQueue.Dequeue();
        bool isNPC2Speaking = isNPC2SpeakingQueue.Dequeue();

        if (isPlayerSpeaking)
        {
            nameText.text = "Lucien";
            characterPortrait.sprite = playerPortrait;
        }
        else if (isAldricSpeaking)
        {
            nameText.text = npc1Name;
            characterPortrait.sprite = npc1Portrait;
        }
        else if (isNPC2Speaking)
        {
            nameText.text = npc2Name;
            characterPortrait.sprite = npc2Portrait;
        }
        else
        {
            nameText.text = npc3Name;
            characterPortrait.sprite = npc3Portrait;
        }

        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
    }

    IEnumerator TypeSentence(string sentence)
    {
        dialogueText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(0.008f);
        }
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

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            DisplayNextSentence();
        }
    }
}