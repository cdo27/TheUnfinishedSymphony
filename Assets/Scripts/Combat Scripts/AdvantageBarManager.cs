using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdvantageBarManager : MonoBehaviour
{
    public CombatStateManager combatStateManager;
    public PlayerManager playerManager;
    public RectTransform leftBar; // Reference to the left bar's RectTransform
    public RectTransform rightBar; // Reference to the right bar's RectTransform
    public RectTransform energyClash; // Reference to the energy clashing image's RectTransform
    private float advantage = 30f; // Starts in the middle (0 to 100)
    private float barWidth; // Total width of the advantage bar

    private int totalAttackNotes = 1; // Prevent divide by zero
    private int totalDefenseNotes = 1;

    private float attackUnit; // Scaled attack movement
    private float defenseUnit; // Scaled defense penalty

    private float decayRate; // Dynamic decay rate
    private float minimumAdvantage = 30f; // Stops decay below this threshold
    private float winThreshold = 51f; // Winning threshold

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

        // Apply potion effect if the player has it
        if (playerManager.GetPurchasedItems().Contains(2)) // Assuming item ID 2 is the potion
        {
            advantage = 50f; // Increase advantage by 20 when the potion is used
        }

        totalAttackNotes = Mathf.Max(attackNotes, 1);
        totalDefenseNotes = Mathf.Max(defenseNotes, 1);

        // Ensure 90% perfect hits overcome decay and reach 100
        float maxPushRight = 70f; // Moves from center (50) to full right (100)
        attackUnit = maxPushRight / (0.6f * totalAttackNotes); // Adjusted for 60% success
        defenseUnit = (maxPushRight / 1.4f) / (0.6f * totalDefenseNotes);

        // Set decay rate so it applies constant pressure
        decayRate = maxPushRight / (combatStateManager.currentSong.songLength * 12.5f);

        // Calculate the usable width for the bar (subtract padding from both sides)
        barWidth = leftBar.rect.width + rightBar.rect.width;


        UpdateUI();
        StartDecay(); // Start decay when the bar is initialized
    }

    // Start decay manually when the game logic determines it's time
    private void StartDecay()
    {
        if (!decayStarted)
        {
            decayStarted = true;
            decayCoroutine = StartCoroutine(DecayAdvantage());
        }
    }

    // Apply decay over time
    private IEnumerator DecayAdvantage()
    {
        while (true)
        {
            if (advantage > minimumAdvantage)
            {
                advantage -= decayRate;
                advantage = Mathf.Clamp(advantage, 0f, 100f);
                UpdateUI();
            }
            yield return new WaitForSeconds(1f);
        }
    }

    // Call this function to handle attacks
    public void HandleAttack(string hitType)
    {
        float moveAmount = 0f;

        switch (hitType)
        {
            case "Perfect":
                moveAmount = attackUnit;
                break;
            case "NearMiss":
                moveAmount = attackUnit * 0.75f;
                break;
            case "Miss":
                moveAmount = 0f;
                break;
        }

        advantage = Mathf.Clamp(advantage + moveAmount, 0f, 100f);
        UpdateUI();
    }

    public void HandleDefense(string hitType)
    {
        float moveAmount = 0f;

        switch (hitType)
        {
            case "PerfectGuard":
                moveAmount = 0f; // No penalty
                break;
            case "WeakBlock":
                moveAmount = defenseUnit * 0.25f; // Partial penalty
                break;
            case "Miss":
                moveAmount = defenseUnit; // Full penalty
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
        // Ensure barWidth is always updated dynamically
        barWidth = leftBar.rect.width + rightBar.rect.width;

        // Calculate left and right bar sizes based on the advantage
        float leftBarSize = (advantage / 100f) * barWidth;
        float rightBarSize = barWidth - leftBarSize; // Ensure it always adds up to barWidth

        // Apply the new sizes
        leftBar.sizeDelta = new Vector2(leftBarSize, leftBar.sizeDelta.y);
        rightBar.sizeDelta = new Vector2(rightBarSize, rightBar.sizeDelta.y);

        // Ensure Energy Clash is centered correctly
        float energyPosition = leftBarSize - (barWidth / 2) + (energyClash.rect.width); // Adjusted for clash point width
        energyClash.anchoredPosition = new Vector2(energyPosition, energyClash.anchoredPosition.y);
    }

}