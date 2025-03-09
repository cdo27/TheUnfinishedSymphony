using UnityEditor;
using UnityEngine;
using System.Collections;

public class Note : MonoBehaviour
{
    private SpriteRenderer spriteRenderer; // Reference to the note's sprite renderer
    public Sprite redNoteSprite;  
    public Sprite greenNoteSprite;
    public Sprite purpleNoteSprite;
    public Sprite redAttackSprite;
    public Sprite greenAttackSprite;
    public Sprite purpleAttackSprite;

    public int noteType;
    private int mode;

    private Vector2 targetPosition;
    private Vector2 destroyPosition;
    private bool hasReachedTarget = false;
    private bool duringDestroyAnimation = false;
    private float speed;

    private BeatManager beatManager;
    private AudioManager audioManager;
    private AdvantageBarManager advantageBarManager;

    float hitTolerance = 0.1f; // Total time window to register a hit
    float perfectHitThreshold = 0.05f; // Smaller window for a perfect hit
    private double targetHitTime;

    // Variables for Defend Mode
    private bool isCharging = false; // Whether the note is charging towards the player

    // Initialize method
    public void Initialize(BeatData beat, int mode, Vector2 targetPosition, Vector2 destroyPosition, float speed, BeatManager beatManager)
    {
        this.noteType = beat.noteType;
        this.mode = mode;
        this.targetPosition = targetPosition;
        this.destroyPosition = destroyPosition;
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
                return 2;
            }
            else
            {    
                return 1;
            }
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
        if (transform.position.x <= destroyPosition.x) 
        {
            Destroy(gameObject);
        }
    }

    //when note hit, the note will rush to the enemy before being destroyed if attack note.
    public void handleHit(Vector3 enemyPosition)
    {
        if (mode == 2) // Defend note
        {
            duringDestroyAnimation = true;
            StartCoroutine(defendDestroyAnimation());
        }
        else if (mode == 1) // Attack note
        {
            switch (noteType)
            {
                case 0:
                    spriteRenderer.sprite = redAttackSprite;
                    break;
                case 1:
                    spriteRenderer.sprite = greenAttackSprite;
                    break;
                case 2:
                    spriteRenderer.sprite = purpleAttackSprite;
                    break;
            }
            
            // Move the note instantly up by 3 units
            transform.position = new Vector3(transform.position.x - 2, transform.position.y + 3, transform.position.z);

            // Animate the note moving towards the enemy
            StartCoroutine(MoveTowardsEnemy(enemyPosition));
        }
    }

    private IEnumerator defendDestroyAnimation()
    {
        float duration = 0.1f; // Duration of the effect
        float elapsedTime = 0f;
        Vector3 originalScale = transform.localScale;
        Vector3 targetScale = originalScale * 3f; // Expand size by 1.5x

        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        Color originalColor = sr.color;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;

            // Fade out
            sr.color = new Color(originalColor.r, originalColor.g, originalColor.b, Mathf.Lerp(1f, 0f, t));

            // Expand size
            transform.localScale = Vector3.Lerp(originalScale, targetScale, t);

            yield return null;
        }

        Destroy(gameObject); // Remove the note after animation
    }

    private IEnumerator MoveTowardsEnemy(Vector3 enemyPosition)
    {
        float speed = 10f; // Adjust speed as needed
        float timeLimit = 1.2f; // Time limit for the failsafe
        float startTime = Time.time; // Record the start time

        while (Vector3.Distance(transform.position, new Vector3(enemyPosition.x, enemyPosition.y, transform.position.z)) > 0.1f)
        {
            // Move towards the target position
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(enemyPosition.x, enemyPosition.y, transform.position.z), 20 * Time.deltaTime);

            // Check if time limit is exceeded
            if (Time.time - startTime > timeLimit)
            {
                Debug.Log("Failsafe activated: Destroying object");
                Destroy(gameObject); // Destroy the object after the time limit
                yield break; // Exit the coroutine early
            }

            yield return null;
        }

        Destroy(gameObject); // Destroy after reaching the enemy
    }

    void defendNoteUpdate()
    {
        double currentDspTime = AudioSettings.dspTime;

        // Determine when to start charging (currently set to 0.2s before the hit time)
        double chargeStartTime = targetHitTime - 0.2;
        if(duringDestroyAnimation == false)
        {
            if (currentDspTime >= chargeStartTime && !isCharging)
            {
                isCharging = true; // Start charging

                float timeToTarget = (float)(targetHitTime - currentDspTime);
                if (timeToTarget > 0)
                {
                    speed = Vector3.Distance(transform.position, targetPosition) / timeToTarget;
                }
            }

            // Move the note towards the target position first
            if (isCharging && !hasReachedTarget)
            {
                transform.position = Vector3.MoveTowards(
                    transform.position,
                    new Vector3(targetPosition.x, targetPosition.y, transform.position.z),
                    speed * Time.deltaTime
                );

                // Check if it has reached the target position
                if (transform.position == new Vector3(targetPosition.x, targetPosition.y, transform.position.z))
                {
                    hasReachedTarget = true; // Mark as reached
                }
            }
            // After reaching target position, move towards destroy position
            else if (isCharging && hasReachedTarget)
            {
                transform.position = Vector3.MoveTowards(
                    transform.position,
                    new Vector3(destroyPosition.x, destroyPosition.y, transform.position.z),
                    speed * Time.deltaTime
                );

                // Destroy the note at the destroy position
                if (transform.position == new Vector3(destroyPosition.x, destroyPosition.y, transform.position.z))
                {
                    advantageBarManager.HandleDefense("Miss");
                    audioManager.playPlayerDamagedSound();

                    //play hurt animation
                    beatManager.combatStateManager.combatAnimationManager.LucienPlayHurtAnimation();
                    Destroy(gameObject);
                }
            }
        }
        
    }
}