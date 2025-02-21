using UnityEditor;
using UnityEngine;

public class Note : MonoBehaviour
{
    private SpriteRenderer spriteRenderer; // Reference to the note's sprite renderer
    public Sprite redNoteSprite;  
    public Sprite greenNoteSprite;
    public Sprite purpleNoteSprite;

    public int noteType;
    private int mode;

    private Vector2 targetPosition;
    private float speed;

    private BeatManager beatManager;
    private AudioManager audioManager;
    private AdvantageBarManager advantageBarManager;

    float hitTolerance = 0.2f; // Total time window to register a hit
    float perfectHitThreshold = 0.1f; // Smaller window for a perfect hit
    private double targetHitTime;

    // Variables for Defend Mode
    private bool isCharging = false; // Whether the note is charging towards the player

    // Initialize method
    public void Initialize(BeatData beat, int mode, Vector2 targetPosition, float speed, BeatManager beatManager)
    {
        this.noteType = beat.noteType;
        this.mode = mode;
        this.targetPosition = targetPosition;
        this.speed = speed; // The speed at which the note will move
        this.beatManager = beatManager; // Reference to the BeatManager for DSP timing

        spriteRenderer = GetComponent<SpriteRenderer>(); // Get reference to the sprite renderer
        //assign proper sprite according to noteType
        switch (noteType)
        {
            case 0:
                spriteRenderer.sprite = redNoteSprite;
                break;
            case 1:
                spriteRenderer.sprite = greenNoteSprite;
                break;
            case 2:
                spriteRenderer.sprite = purpleNoteSprite;
                break;
            default:
                Debug.LogWarning("Invalid note type: " + noteType);
                break;
        }

        // Get AudioManager reference
        audioManager = FindObjectOfType<AudioManager>();
        if (audioManager == null)
        {
            Debug.LogError("AudioManager not found in the scene!");
        }

        // Get AudioManager reference
        advantageBarManager = FindObjectOfType<AdvantageBarManager>();
        if (advantageBarManager == null)
        {
            Debug.LogError("advantageBarManager not found in the scene!");
        }

        // Calculate the target hit time of this note based on the beat manager's DSP time
        targetHitTime = beatManager.GetDspTimeForBeat(beat.beatTime);
    }

    public int checkIfHit()
    {
        // Check if the note is within the tolerance range of the target DSP time (timing-based hit)
        double currentDspTime = AudioSettings.dspTime;

        // If the current DSP time is within tolerance of the target hit time, change color to red
        // Check if the note is close to the target hit time
        float timeDifference = Mathf.Abs((float)(currentDspTime - targetHitTime));
        if (timeDifference <= hitTolerance)
        {
            if (timeDifference <= perfectHitThreshold) // Define a smaller threshold for perfect hits
            {
                spriteRenderer.color = Color.green; // Perfect hit zone
                return 2;
            }
            else
            {
                spriteRenderer.color = Color.red; // In range but slightly off
                return 1;
            }
        }
        else
        {
            spriteRenderer.color = Color.white; // Out of range
        }
        return 0;
    }

    void Update()
    {
        if (mode == 1)
        {
            attackNoteUpdate();
        }
        else if (mode == 2)
        {
            defendNoteUpdate();
        }
    }

    void attackNoteUpdate()
    {
        // Move the note towards the target position (based on x and y)
        transform.position = Vector3.MoveTowards(transform.position, new Vector3(targetPosition.x-3, transform.position.y, transform.position.z), speed * Time.deltaTime);

        // Check if the note has passed the target position by checking its x position
        if (transform.position.x <= targetPosition.x - 2) // Adjust end point as needed
        {
            Destroy(gameObject); // Remove the note when it reaches this point
        }
    }

    void defendNoteUpdate()
    {
        // Check if the current DSP time is close to the target hit time
        double currentDspTime = AudioSettings.dspTime;

        // If current DSP time is within tolerance range of the target hit time, start charging
        if (Mathf.Abs((float)(currentDspTime - targetHitTime)) <= hitTolerance && !isCharging)
        {
            isCharging = true; // Start charging
        }

        // If we are in charging mode, move the note instantly towards the target position
        if (isCharging)
        {
            // Move the note instantly towards the target position (with high speed)
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(targetPosition.x, targetPosition.y, transform.position.z), 40 * Time.deltaTime);

            // Optionally, destroy the note when it reaches the target position
            if (transform.position == new Vector3(targetPosition.x, targetPosition.y, transform.position.z))
            {
                advantageBarManager.HandleDefense("Miss");
                Destroy(gameObject);
            }
        }
    }
}

/** FOR TESTING
       double currentDspTime = AudioSettings.dspTime;
       float timeDifference = Mathf.Abs((float)(currentDspTime - targetHitTime));
       if (timeDifference <= hitTolerance)
       {
           if (timeDifference <= perfectHitThreshold) // Define a smaller threshold for perfect hits
           {
               spriteRenderer.color = Color.green; // Perfect hit zone
            
           }
           else
           {
               spriteRenderer.color = Color.red; // In range but slightly off
          
           }
       }
       else
       {
           spriteRenderer.color = Color.white; // Out of range
       }
       */