using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdvantageBarManager : MonoBehaviour
{

    public PlayerManager playerManager;
    public RectTransform leftBar; // Reference to the left bar's RectTransform
    public RectTransform rightBar; // Reference to the right bar's RectTransform
    public RectTransform energyClash; // Reference to the energy clashing image's RectTransform
    private float advantage = 50f; // Starts in the middle (0 to 100)
    private float barWidth = 930f; // Total width of the advantage bar

    private int totalAttackNotes = 1; // Default to prevent divide by zero
    private int totalDefenseNotes = 1;

    private float attackUnit; // How much each attack note moves the bar
    private float defenseUnit; // How much each defense note moves the bar

    private float decayRate = 0.1f; // Rate at which the enemy meter decays per second
    private float minimumAdvantage = 20f; // Minimum advantage before the decay stops
    private float winThreshold = 51f; // Advantage required to win (over half)

    private Coroutine decayCoroutine; // Reference to the decay coroutine
    private bool decayStarted = false; // Track if decay has been initiated

    // Call this function to initialize the attack and defense notes
    public void InitializeBar(int attackNotes, int defenseNotes)
    {
        
        playerManager = GameObject.Find("PlayerManager").GetComponent<PlayerManager>();

        // Optionally, check if the playerManager was found
        if (playerManager == null)
        {
            Debug.LogError("PlayerManager not found in the scene.");
        }
    
        advantage = 50f;

       
        // Apply potion effect if the player has it
        if (playerManager.GetPurchasedItems().Contains(2)) // Assuming item ID 2 is the potion
        {
            advantage = 70f; // Increase advantage by 20 when the potion is used
        }
      
        totalAttackNotes = Mathf.Max(attackNotes, 1); // Avoid division by zero
        totalDefenseNotes = Mathf.Max(defenseNotes, 1);

        // Set scaling factors so that hitting every note perfectly would fully shift the bar
        attackUnit = 50f / totalAttackNotes;  // 50 means shifting from center to one extreme
        defenseUnit = 50f / totalDefenseNotes;

        UpdateUI();
        StartDecay(); // Start decay when the bar is initialized
    }

    // Start decay manually when the game logic determines it's time
    private void StartDecay()
    {
        if (!decayStarted) // Prevent multiple coroutines from running
        {
            decayStarted = true;
            decayCoroutine = StartCoroutine(DecayAdvantage());
        }
    }

    // Coroutine to decay enemy advantage over time
    private IEnumerator DecayAdvantage()
    {
        while (true)
        {
            // Decay the enemy advantage if it's above the minimum threshold
            if (advantage > minimumAdvantage)
            {
                advantage -= decayRate; // Apply decay
                advantage = Mathf.Clamp(advantage, 0f, 100f); // Ensure it's within bounds
                UpdateUI();
            }
            yield return new WaitForSeconds(1f); // Adjust this value to change decay frequency
        }
    }

    // Call this function to handle attacks
    public void HandleAttack(string hitType)
    {
        float moveAmount = 0f;
        switch (hitType)
        {
            case "Perfect":
                moveAmount = attackUnit * 1.5f; // Perfect hits push further
                break;
            case "NearMiss":
                moveAmount = attackUnit * 0.75f; // Slight push
                break;
            case "Miss":
                moveAmount = 0f; // No impact
                break;
        }

        advantage = Mathf.Clamp(advantage + moveAmount, 0f, 100f);
        UpdateUI();
    }

    // Call this function to handle defense actions
    public void HandleDefense(string hitType)
    {
        float moveAmount = 0f;
        switch (hitType)
        {
            case "PerfectGuard":
                moveAmount = 0f; // No penalty for perfect block
                break;
            case "WeakBlock":
                moveAmount = defenseUnit * 0.5f; // Partial penalty
                break;
            case "Miss":
                moveAmount = defenseUnit * 1.5f; // Full penalty
                break;
        }
        
               // Apply armor effect if the player has it
               if (playerManager.GetPurchasedItems().Contains(1)) // Assuming item ID 1 is the armor
               {
                   moveAmount *= 0.5f; // Halve the damage taken during the defense phase
               }
       
        advantage = Mathf.Clamp(advantage - moveAmount, 0f, 100f);

        UpdateUI();

    }

    // Check if the player has won the game
    public bool CheckVictoryCondition()
    {
        if (advantage >= winThreshold) // Returns true if the player has won
        {
            StopDecay(); // Stop decay when the player wins
            return true;
        }
        return false;
    }

    // Stop the decay coroutine
    public void StopDecay()
    {
        if (decayCoroutine != null)
        {
            StopCoroutine(decayCoroutine);
            decayCoroutine = null; // Clear reference
        }
        decayStarted = false; // Reset flag
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
        energyClash.anchoredPosition = new Vector2(energyPosition, energyClash.anchoredPosition.y);
    }
 
}