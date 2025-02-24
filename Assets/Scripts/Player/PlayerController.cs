using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerController: MonoBehaviour
{
    public bool canMove = true;
    private const float playerSpeed = 13f;
    private Vector3 moveDir;
    private Rigidbody2D playerRigidbody2D;
    private Animator animator;

    private GameManager gameManager;
    private PlayerManager playerManager;

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
        canMove = false;
        animator.SetBool("IsMoving", false);
    }

    public void StartPlayerMovement(){
        canMove = true;
    }
    private void HandleMovement(){
        float moveX = 0f;
        float moveY = 0f;

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)){
            moveY = +1f;
        }
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)){
            moveY = -1f;
        }
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)){
            moveX = -1f;
            animator.SetFloat("FacingDirection", -1);
        }
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)){
            moveX = +1f;
            animator.SetFloat("FacingDirection", 1); 
        }

        moveDir = new Vector3(moveX, moveY).normalized;

        if (moveX != 0)
        {
            animator.SetFloat("MoveX", moveX);
        }

        animator.SetBool("isMoving", moveX != 0 || moveY != 0);
    }

    private void FixedUpdate(){ //updates player position with moveDir
        playerRigidbody2D.MovePosition(transform.position + moveDir * playerSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {

        if (collider.CompareTag("Coin")) // object has the tag "Coin"
        {   
            Debug.Log("Collected Coin!");
            CollectCoin(collider.gameObject);
        }
    }

    void CollectCoin(GameObject coin)
    {
        playerManager.UpdateCoinCount(1);
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
