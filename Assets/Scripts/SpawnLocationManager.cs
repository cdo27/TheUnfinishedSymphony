using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SpawnLocationManager : MonoBehaviour
{
    public GameManager gameManager;

    [Header("EscapeHallwaySpawnPoints")]
    public GameObject playerObject;
    public GameObject leftSpawn;
    public GameObject rightSpawn;
    public GameObject trapSpawn;

    [Header("HallwaySpawnPoints")]
    public GameObject playerObjectHallway;
    public GameObject hallwaySpawn;
    public GameObject room1Spawn;
    public GameObject room2Spawn;
    public GameObject room3Spawn;

    [Header("EntranceSpawnPoints")]
    public GameObject playerObjectEntrance;
    public GameObject leftHallwaySpawn;
    public GameObject upRoomSpawn;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();

        //coming from direction of hallway, left spawn point
        if (SceneManager.GetActiveScene().name == "EscapeHallway"){
            if(gameManager.lastScene == "HallwayCutscene" || gameManager.lastScene == "Hallway") playerObject.transform.position = leftSpawn.transform.position;
            if(gameManager.lastScene == "Entrance" || gameManager.lastScene == "Intro") playerObject.transform.position = rightSpawn.transform.position;
            if(gameManager.lastScene == "EscapeRoom") playerObject.transform.position = trapSpawn.transform.position;


        } //Going into Hallway
        else if (SceneManager.GetActiveScene().name == "Hallway"){
            if(gameManager.lastScene == "EscapeHallway") playerObjectHallway.transform.position = hallwaySpawn.transform.position;
            if(gameManager.lastScene == "FirstWing") playerObjectHallway.transform.position = room1Spawn.transform.position;
            if(gameManager.lastScene == "SecondWing") playerObjectHallway.transform.position = room2Spawn.transform.position;
            if(gameManager.lastScene == "ThirdWing") playerObjectHallway.transform.position = room3Spawn.transform.position;
        }//Going into Entrance
        else if (SceneManager.GetActiveScene().name == "Entrance"){
            if(gameManager.lastScene == "EscapeHallway") playerObjectEntrance.transform.position = leftHallwaySpawn.transform.position;
            if(gameManager.lastScene == "BossRoom") playerObjectEntrance.transform.position = upRoomSpawn.transform.position;
   
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
