using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NoteSpawner : MonoBehaviour
{

    public Scene combatScene;

    //template note object that will be cloned
    public GameObject notePrefab;

    //the attack bar
    public GameObject folderObject;

    //player position
    public GameObject player;

    //enemy position
    public GameObject enemy;

    //the attack hit point (where you are supposed to hit the note)
    public GameObject hitPointObject;

    //the defend hit point (where you are supposed to hit the note)
    public GameObject defendPointObject;

    //point where attack notes get automatically destroyed when players miss
    public GameObject attackDestroyObject;

    //point where defend notes get automatically destroyed when players miss
    public GameObject defendDestroyObject;

    //spawn point
    public GameObject spawnPointObject;

    //beatmanager
    private BeatManager beatManager;

    //how many beats it take for note to move from spawn point to the hit point (effectively it controls speed of how fast the note is moving)
    public float attackBeatsToHitPoint = 4;
    private float noteSpeed;

    void Start()
    {
        combatScene = SceneManager.GetSceneByName("Combat");

        //find beatmanager
        beatManager = FindObjectOfType<BeatManager>(); // Get reference to BeatManager
        if (beatManager == null)
        {
            Debug.LogError("BeatManager not found in the scene!");
        }
    }

    //before spawning notes, beat manager will read the song's BPM and use this function to calculate the note speed
    public void setNoteSpeed()
    {
        float crotchet = beatManager.getCrotchet(); 
        float timeToHitPoint = attackBeatsToHitPoint * crotchet;

        // Calculate distance from spawn to hit point
        float spawnX = spawnPointObject.transform.position.x;
        float hitPointX = hitPointObject.transform.position.x;
        float distanceToHitPoint = spawnX - hitPointX;

        // Calculate note speed to ensure precise arrival
        noteSpeed = distanceToHitPoint / timeToHitPoint;
    }

    public Note SpawnNote(BeatData beat)
    {
            //Debug.Log("spawning attack mode notes");
            //set the spawn point, x equal some arbitary spawn point (adjust), y is just the y value of the attack bar object
            Vector2 folderPosition = folderObject.transform.position;
            Vector2 targetPosition = hitPointObject.transform.position;
            Vector2 destroyPosition = attackDestroyObject.transform.position;
            float spawnX = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, 0)).x + 1;
            //Vector2 spawnPosition = new Vector2(spawnX, targetPosition.y);
            Vector2 spawnPosition = spawnPointObject.transform.position;

        //instantiate the note
        GameObject newNote = Instantiate(notePrefab, spawnPosition, Quaternion.identity);
            Note note = newNote.GetComponent<Note>();
            if (combatScene.IsValid())
            {
                SceneManager.MoveGameObjectToScene(note.gameObject, combatScene);
            }
            note.Initialize(beat, 1, targetPosition, destroyPosition, noteSpeed, beatManager);

            return note; 
    }

    public Note SpawnDefendNote(BeatData beat, int position, List<BeatData> beatList, float startTime, float endTime)
    {
        Vector2 characterPosition = enemy.transform.position;

        // Arc parameters
        float radiusX = 1f;
        float radiusY = 2.5f;
        float arcStartAngle = Mathf.PI / 2.2f;  // Start of arc (~81 degrees)
        float arcEndAngle = -Mathf.PI / 2.2f;   // End of arc (~-81 degrees)

        // Shift the arc to the right by adjusting the start position on the x-axis
        float arcOffsetX = 1.0f; // Adjust this value to move the arc to the right

        // Normalize the beatTime relative to startTime and endTime
        float normalizedBeatTime = Mathf.InverseLerp(startTime, endTime, beat.beatTime);

        // Interpolate the angle based on the normalized beatTime
        float finalAngle = Mathf.Lerp(arcStartAngle, arcEndAngle, normalizedBeatTime);

        // Compute the note's position based on this angle
        float xPosition = characterPosition.x + Mathf.Cos(finalAngle) * radiusX + arcOffsetX;
        float yPosition = characterPosition.y + Mathf.Sin(finalAngle) * radiusY;

        // Create the spawn position
        Vector2 spawnPosition = new Vector2(xPosition, yPosition);
        Vector2 destroyPosition = defendDestroyObject.transform.position;

        // Instantiate the note
        GameObject newNote = Instantiate(notePrefab, spawnPosition, Quaternion.identity);
        Note note = newNote.GetComponent<Note>();

        // Move the note to the combat scene if valid
        if (combatScene.IsValid())
        {
            SceneManager.MoveGameObjectToScene(note.gameObject, combatScene);
        }

        // Initialize the note with the correct parameters
        note.Initialize(beat, 2, defendPointObject.transform.position, destroyPosition, 0, beatManager);

        return note;
    }
}
