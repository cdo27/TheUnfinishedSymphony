using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdvantageBarManager : MonoBehaviour
{
    public RectTransform leftBar; // Reference to the left bar's RectTransform
    public RectTransform rightBar; // Reference to the right bar's RectTransform
    //public RectTransform energyClash; // Reference to the energy clashing image's RectTransform
    private float advantage = 50f; // Starts in the middle (0 to 100)
    private float barWidth = 1000f; // Total width of the advantage bar

    // Call this function to initialize the attack and defense notes
    public void InitializeBar(int totalAttackNotes, int totalDefenseNotes)
    {
        // Optionally set any other initialization parameters
        UpdateUI();
    }

    // Call this function to handle attacks
    public void HandleAttack(string hitType)
    {
        float moveAmount = 0f;
        Debug.Log("handling attack!");
        switch (hitType)
        {
            case "Perfect":
                moveAmount = 1.5f; // Strong attack, adjust the scale
                break;
            case "NearMiss":
                moveAmount = 0.75f; // Slight attack
                break;
            case "Miss":
                moveAmount = 0f; // No attack
                break;
        }

        // Adjust advantage value and clamp between 0 and 100
        advantage = Mathf.Clamp(advantage + moveAmount, 0f, 100f);
        UpdateUI();
    }

    // Call this function to handle defense actions
    public void HandleDefense(bool blocked)
    {
        if (!blocked) // Failed block
        {
            float penalty = 5f; // Define a penalty for failing to block
            advantage = Mathf.Clamp(advantage - penalty, 0f, 100f); // Reduce advantage
        }

        UpdateUI();
    }

    // Update the UI to reflect the current advantage value
    private void UpdateUI()
    {
        // Calculate left and right bar sizes based on the advantage
        float leftBarSize = (advantage / 100) * barWidth;
        float rightBarSize = ((100 - advantage) / 100) * barWidth;

        // Update the width of the bars
        leftBar.sizeDelta = new Vector2(leftBarSize, leftBar.sizeDelta.y);
        rightBar.sizeDelta = new Vector2(rightBarSize, rightBar.sizeDelta.y);

        // Update the position of the energy clashing image
        float energyPosition = (advantage / 100) * barWidth - (barWidth / 2); // Centering it
        //energyClash.anchoredPosition = new Vector2(energyPosition, energyClash.anchoredPosition.y);
    }
}
