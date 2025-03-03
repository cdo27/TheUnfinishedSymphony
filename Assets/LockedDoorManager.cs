using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockedDoorManager : MonoBehaviour
{
    private GameManager gameManager;

    //Hallway Maps
    public Sprite FirstDoorUnlocked;
    public Sprite SecondDoorUnlocked;
    public Sprite ThirdDoorUnlocked;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (gameManager != null)
        {
            spriteRenderer.sprite = FirstDoorUnlocked;

            if (gameManager.hasCompletedPuzzle1 && gameManager.hasCompletedCombat1)
            {
                spriteRenderer.sprite = SecondDoorUnlocked;
            }
            
            if (gameManager.hasCompletedPuzzle2 && gameManager.hasCompletedCombat2)
            {
                spriteRenderer.sprite = ThirdDoorUnlocked;
            }
        }
    }
}