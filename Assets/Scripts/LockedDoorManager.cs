using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockedDoorManager : MonoBehaviour
{
    private GameManager gameManager;
    [Header("Entrance Maps")]
    public Sprite HallwayDoorUnlocked;

    [Header("Hallway Maps")]
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
            //Entrance
            if(HallwayDoorUnlocked != null){
                if (gameManager.hasCompletedPuzzleTut)
                {
                    spriteRenderer.sprite = HallwayDoorUnlocked;
                }
            }



            //Hallway
            if(FirstDoorUnlocked != null) spriteRenderer.sprite = FirstDoorUnlocked;

            if(SecondDoorUnlocked != null){
                if (gameManager.hasCompletedPuzzle1 && gameManager.hasCompletedCombat1)
                {
                    spriteRenderer.sprite = SecondDoorUnlocked;
                }
            }

            if(ThirdDoorUnlocked != null){
                if (gameManager.hasCompletedPuzzle2 && gameManager.hasCompletedCombat2)
                {
                    spriteRenderer.sprite = ThirdDoorUnlocked;
                }
            }
        }
    }
}