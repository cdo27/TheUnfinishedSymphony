using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerController: MonoBehaviour
{
    public bool canMove = true;
    private const float playerSpeed = 11f;
    private Vector3 moveDir;
    private Rigidbody2D playerRigidbody2D;
    public Animator animator;

    private GameManager gameManager;
    private PlayerManager playerManager;
    private DialogueManager dialogueManager;
    private AudioManager audioManager;
    private CoinManager coinManager;

    public float interactRange = 1f;
    public KeyCode interactKey = KeyCode.F; //F to interact
    public GameObject interactionText; //Press F text
    private Interactable currentInteractable; //Track nearest interactable

    void Start()
    {
        playerRigidbody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        gameManager = FindObjectOfType<GameManager>();
        playerManager = FindObjectOfType<PlayerManager>();
        audioManager = FindObjectOfType<AudioManager>(); 
        coinManager = FindObjectOfType<CoinManager>();
        dialogueManager = FindObjectOfType<DialogueManager>();

         if (interactionText != null)
        {
            interactionText.gameObject.SetActive(false);
        }
    }

    void Update()
    {
         if (gameManager == null)
    {
        gameManager = FindObjectOfType<GameManager>();
        if (gameManager == null)
        {
            return; // Wait until it exists
        }
    }
        if (gameManager.currentState == GameManager.GameState.Game){
            HandleMovement();
            DetectInteractable();
            
        } else{
            interactionText.gameObject.SetActive(false);
            
        }

        if (Input.GetKeyDown(interactKey) && currentInteractable != null)
        {
            currentInteractable.Interact();
        }
        
    }

    //----------------Movement Code-----------------------------------------
    public void StopPlayerMovement(){
        Debug.Log("stopping player movement");
        canMove = false;
        animator.SetBool("isMoving", false);
        animator.SetInteger("MoveX", 0);
        animator.SetInteger("MoveY", 0);
    }

    public void StartPlayerMovement(){
        canMove = true;
    }
    private void HandleMovement()
    {
        int moveX = 0;
        int moveY = 0;

        if (canMove)
        {
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
            {
                moveY = 1;
            }
            if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
            {
                moveY = -1;
            }
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            {
                moveX = -1;
            }
            if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            {
                moveX = 1;
            }
        }

        moveDir = new Vector3(moveX, moveY).normalized;

        // Update Animator parameters
        animator.SetInteger("MoveX", moveX);
        animator.SetInteger("MoveY", moveY);
        animator.SetBool("isMoving", moveX != 0 || moveY != 0);

        if (moveX != 0 || moveY != 0)
        {
            audioManager.PlayWalkingSound();
        }
        else
        {
            audioManager.StopWalkingSound();
        }

        //Debug.Log($"MoveX: {moveX}, MoveY: {moveY}");
    }


    private void FixedUpdate(){ //updates player position with moveDir
        if (canMove) 
        {
            playerRigidbody2D.MovePosition(transform.position + moveDir * playerSpeed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {

        if (collider.CompareTag("Coin")) // object has the tag "Coin"
        {   
            Debug.Log("Collected Coin!");
            CollectCoin(collider.gameObject);
        }
        else if (collider.CompareTag("UpdateArea")) //scene trigger
        {
            if (gameManager != null)
            {
                gameManager.PlayerPassedArea();
            }
        }
    }

    void CollectCoin(GameObject coin)
    {
        Coin coinScript = coin.GetComponent<Coin>();
        if (coinScript != null)
        {
            CoinManager.Instance.CollectCoin(coinScript.coinID);
        }

        playerManager.UpdateCoinCount(1);
        FindObjectOfType<AudioManager>().PlayCoinCollectSound();
        Destroy(coin);

    }


    //----------------Interact Code-----------------------------------------

    void DetectInteractable()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, interactRange);
        Interactable closestInteractable = null;
        float closestDistance = float.MaxValue;

        foreach (Collider2D collider in colliders)
        {
            Interactable interactable = collider.GetComponent<Interactable>();
            if (interactable != null && !interactable.isInteracting)
            {
                float distance = Vector2.Distance(transform.position, collider.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestInteractable = interactable;
                }
            }
        }

        currentInteractable = closestInteractable;

        // interaction text
        if (interactionText != null)
        {
            if (currentInteractable != null)
            {
                //interactionText.text = "Press F to interact";
                interactionText.gameObject.SetActive(true);
            }
            else
            {
                interactionText.gameObject.SetActive(false);
            }
        }
    }
}
