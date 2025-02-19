using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CombatStateManager : MonoBehaviour
{
    //current song that will be played, when loading combat this should be loaded based on which level
    public Song currentSong;

    public int gameState = 0; // 0 = not started, 1 = attack mode, 2 = defend mode, 98 = victory, 99 = defeat
    public double lastCheckedTime = -1.0; // Track last checked DSP time
    public BeatManager beatManager;
    public GameObject attackBar;

    public GameObject enemy;
    public Sprite thiefSprite;
    public Sprite redSpiritSprite;
    public Sprite yelowSpiritSprite;
    public Sprite blueSpiritSprite;

    // Just for testing
    public TMP_Text modeText;

    void Start()
    {
        //load proper song
        currentSong = new TestSong();

        if (currentSong.songID == 001)
        {
            SpriteRenderer enemySpriteRenderer = enemy.GetComponent<SpriteRenderer>();
            enemySpriteRenderer.sprite = thiefSprite;
        }
    }

    void Update()
    {
        if(gameState == 98)
        {
            if (modeText != null)
            {
                modeText.text = "YOU WIN";
            }
        }

        if (gameState != 98 && gameState != 99 && beatManager.songStarted)
        {
            double currentTime = AudioSettings.dspTime;

            // Update the UI text for debugging
            if (modeText != null)
            {
                modeText.text = "Mode: " + GetModeText();
            }

            if (currentTime > lastCheckedTime) // Ensure we only check once per frame
            {
                lastCheckedTime = currentTime;
                CheckModeSwitch(currentTime);
            }
        }
        
    }

    void CheckModeSwitch(double currentTime)
    {
        // Check if it's time to switch to attack mode
        if (currentSong.attackModeBeats.Count > 0)
        {
            double attackModeTime = beatManager.GetDspTimeForBeat(currentSong.attackModeBeats[0]);
            if (currentTime >= attackModeTime)
            {
                gameState = 1; // Attack mode
                attackBar.SetActive(true);
                Debug.Log("Switched to Attack Mode!");
                currentSong.attackModeBeats.RemoveAt(0); // Remove processed beat
            }
        }

        // Check if it's time to switch to defend mode
        if (currentSong.defendModeBeats.Count > 0)
        {
            double defendModeTime = beatManager.GetDspTimeForBeat(currentSong.defendModeBeats[0]);
            if (currentTime >= defendModeTime)
            {
                gameState = 2; // Defend mode
                attackBar.SetActive(false);
                Debug.Log("Switched to Defend Mode!");
                currentSong.defendModeBeats.RemoveAt(0); // Remove processed beat
            }
        }
    }

    string GetModeText()
    {
        switch (gameState)
        {
            case 1:
                return "Attack Mode";
            case 2:
                return "Defend Mode";
            default:
                return "Waiting...";
        }
    }
}
