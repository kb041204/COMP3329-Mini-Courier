using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu_animation : MonoBehaviour
{
    //This script is responsible for spawning AIs in the main menu at the ***top and bottom lane***

    public GameObject AI_Truck;
    public GameObject AI_Van;
    public GameObject AI_Motorcycle;

    public GameObject target_building;
    private int currCargo;
    public static int currVehicle;
    public static float speed;
    public static bool spawned;

    public GameObject AI_prefab;
    private float AI_timer;
    private int AI_spawnDuration;
    private System.Random ran = new System.Random();

    void spawnAI()
    {
        spawned = true;

        //list all the AI driver to be spawned
        GameObject AI_to_spawn;
        switch(currVehicle)
        {
            case 0:
                AI_to_spawn = AI_Motorcycle;
                speed = 300f;
                break;
            case 1:
                AI_to_spawn = AI_Van;
                speed = 250f;
                break;
            case 2:
                AI_to_spawn = AI_Truck;
                speed = 200f;
                break;
            default:
                AI_to_spawn = AI_Van;
                speed = 250f;
                break;
        }
        Instantiate(AI_to_spawn, new Vector2(-40,682), Quaternion.identity); //spawn the AI
        target_building.GetComponent<SpriteRenderer>().color = GameManager.colours[currCargo]; //change colour
    }

    // Start is called before the first frame update
    void Start()
    {
        //Left hand side traffic
        currCargo = 0;
        currVehicle = 0;
        spawnAI();

        //Right hand side traffic
        Instantiate(AI_prefab, new Vector2(1960, 615), Quaternion.identity);
        AI_timer = 0;
        AI_spawnDuration = ran.Next(3, 9);
    }

    // Update is called once per frame
    void Update()
    {
        //Left hand side traffic
        if(!spawned)
        {
            currCargo = ++currCargo % 26;
            currVehicle = ++currVehicle % 3;
            spawnAI();
        }

        //Right hand side traffic
        AI_timer += Time.deltaTime;
        if (AI_timer >= AI_spawnDuration)
        {
            Instantiate(AI_prefab, new Vector2(1960, 615), Quaternion.identity);
            AI_spawnDuration = ran.Next(3, 9);
            AI_timer = 0f;
        }
    }
}
