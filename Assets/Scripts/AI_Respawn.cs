using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_Respawn : MonoBehaviour
{
    public static int numAIToBeSpawned; 

    public GameObject AI_Prefab_car;

    private Vector2 respawnLocation;
    private bool canRespawn;
    private float cooldownTimer;

    // Start is called before the first frame update
    void Start()
    {
        canRespawn = true;
        numAIToBeSpawned = 0;
        cooldownTimer = 0f;

        //different spawn location for each map
        if (GameManager.map == "Tokyo")
            respawnLocation = new Vector2(1753, 29);
        else if (GameManager.map == "Hong Kong")
            respawnLocation = new Vector2(1796, 42);
        else if (GameManager.map == "New York")
            respawnLocation = new Vector2(1180, 170);
    }

    void Update()
    {
        if (numAIToBeSpawned > 0) //check whether there are AI to be spawned
        {
            cooldownTimer += Time.deltaTime;
            if (canRespawn && cooldownTimer >= 2f) //only spawn AI every 2 seconds and when the sector is clear of traffic
            {
                //Debug.Log("AI Respawned!");
                Instantiate(AI_Prefab_car, respawnLocation, Quaternion.identity);
                numAIToBeSpawned--;
                cooldownTimer = 0f;
            }
        }
    }

    public void OnTriggerStay2D(Collider2D obj) //Therea are other objects in the section near the spawner
    {
        //Debug.Log("Collision!");
        string name = obj.gameObject.name;
        if (name == "AI" || name == "vehicle" || name == "AI(Clone)")
            canRespawn = false;
    }

    public void OnTriggerExit2D(Collider2D obj) //The other objects left the section near the spawner
    {
        //Debug.Log("No more collision!");
        string name = obj.gameObject.name;
        if (name == "AI" || name == "vehicle" || name == "AI(Clone)")
            canRespawn = true;
    }

    public static void respawnAI() //let other scripts to call this function and to spawn a new AI
    {
        numAIToBeSpawned++;
    }
}
