using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; //For TextMeshPro TMP_Text

public class VehicleScript : MonoBehaviour
{
    private float topSpeed = 275f;
    public static int currCargoSize = 0;
    public static int maxCargoSize = 6;
    public static int currTripCargoDelivered = 0;
    private static int vehicleCapacityUpgradeAmount = 1;
    public static List<int> cargoes_pickedup = new List<int>();
    public GameObject[] cargo_indicators = new GameObject[12]; //Maximum 12 cargo indicators

    public static int numSpeedBooster = 0;
    private float speedBoosterTimer = 0f;
    private float speedBoosterTimeLimit = 5f;
    private float speedBoostAmount = 150f;
    private bool isInSpeedBoost = false;

    public static int numTimeExtender = 0;
    private float timeExtenderAmount = 10f;

    public static int playerCollideWithAI = 0; //number of times player collide with an AI

    public bool cargo_menu_opened = false;
    private float cargo_dist_exit_time = 0f;

    private BoxCollider2D boxCol;
    private PolygonCollider2D polyCol;
    private CapsuleCollider2D capCol;

    public static GameObject arrow;
    public static AudioSource engine_sound;

    public void ResetGame()
    {
        arrow.SetActive(false);
        currTripCargoDelivered = 0;
        playerCollideWithAI = 0;
        currCargoSize = 0;
        numSpeedBooster = 0;
        numTimeExtender = 0;
        cargo_dist_exit_time = 0f;
        cargoes_pickedup.Clear();
    }

    void Start()
    {
        boxCol = GetComponent<BoxCollider2D>();
        polyCol = GetComponent<PolygonCollider2D>();
        capCol = GetComponent<CapsuleCollider2D>();

        engine_sound = GameObject.FindWithTag("engine_sound").GetComponent<AudioSource>();

        playerCollideWithAI = 0;

        //Set initial cargo size and speed
        if (GameManager.vehicle == "Motorcycle")
        {
            maxCargoSize = 4;
            topSpeed = 300f;
        } 
        else if (GameManager.vehicle == "Van")
        {
            maxCargoSize = 6;
            topSpeed = 250f;
        }
        else if (GameManager.vehicle == "Truck")
        {
            maxCargoSize = 8;
            topSpeed = 200f;
        }

        //Load all the cargo pickedup indicators into the variable
        for(int i = 0; i < 12; i++)
        {
            string tmp = "cargo_pickedup " + (i + 1).ToString();
            cargo_indicators[i] = GameObject.Find(tmp);
        }

        arrow = GameObject.FindWithTag("arrow");
        arrow.SetActive(true); //display the giant arrow
    }

    void Update()
	{
		Rigidbody2D rb = GetComponent<Rigidbody2D>();
        Vector2 v = rb.velocity;
        float accelAndDecelRate = topSpeed / 75;
        float naturalDecelerationRate = topSpeed / 150;

        bool keypressed = false;
        
        bool wPressed = false;
        bool sPressed = false;

        if (PauseMenuScript.cargoPickupMenuOpened || PauseMenuScript.gameIsPaused)
            return;

        //Detect key pressed and accelerate/decelerate
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            keypressed = true;
            wPressed = true;
            if (v.y + accelAndDecelRate <= topSpeed)
                v.y += accelAndDecelRate;
            else if (v.y > topSpeed)
                v.y = topSpeed;
            if (rb.velocity.y > 0)
                transform.eulerAngles = new Vector3(0, 0, 90);
        }
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            keypressed = true;
            sPressed = true;
            if (v.y - accelAndDecelRate >= -topSpeed)
                v.y -= accelAndDecelRate;
            else if (v.y < -topSpeed)
                v.y = -topSpeed;
            if(rb.velocity.y < 0)
                transform.eulerAngles = new Vector3(0, 0, 270);
        }
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            keypressed = true;
            if (v.x + accelAndDecelRate <= topSpeed)
                v.x += accelAndDecelRate;
            else if (v.x > topSpeed)
                v.x = topSpeed;

            if (v.x > 0)
            {
                rb.rotation = 0;
                transform.eulerAngles = new Vector3(0, 0, 0);
                if (wPressed)
                    transform.eulerAngles = new Vector3(0, 0, 45);
                else if (sPressed)
                    transform.eulerAngles = new Vector3(0, 0, 315);

            }
        }

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            keypressed = true;
            if (v.x - accelAndDecelRate >= -topSpeed)
                v.x -= accelAndDecelRate;
            else if (v.x < -topSpeed)
                v.x = topSpeed;
            if (v.x < 0)
            {
                transform.eulerAngles = new Vector3(0, 180, 0);
                if (wPressed)
                    transform.eulerAngles = new Vector3(0, 180, 45);
                else if (sPressed)
                    transform.eulerAngles = new Vector3(0, 180, 315);

            }
        }

        if (Input.GetKey(KeyCode.B) && !isInSpeedBoost && numSpeedBooster > 0) //Use SpeedBooster
        {
            VehicleScript.arrow.SetActive(false);
            GameObject.FindWithTag("AudioManager").GetComponent<SoundEffects_Script>().playBooster();
            numSpeedBooster--;
            isInSpeedBoost = true;
            topSpeed += speedBoostAmount;
        }

        if (Input.GetKeyDown(KeyCode.N) && numTimeExtender > 0) //Use Time Extender
        {
            VehicleScript.arrow.SetActive(false);
            GameObject.FindWithTag("AudioManager").GetComponent<SoundEffects_Script>().playTimeExtender();
            numTimeExtender--;
            foreach (int cargo in GameManager.cargoes_active)
                GameObject.Find("Cargo_" + cargo.ToString()).GetComponent<SpawnerScript>().IncreaseTimeLimit(timeExtenderAmount);
        }

        //Natural deceleration
        if (!keypressed)
        {
            if (v.x > 0)
                v.x -= naturalDecelerationRate;
            else if (v.x < 0)
                v.x += naturalDecelerationRate;
            if (v.y > 0)
                v.y -= naturalDecelerationRate;
            else if (v.y < 0)
                v.y += naturalDecelerationRate;
        }
        else //Pressed any key
        {
            arrow.SetActive(false);
            if(!engine_sound.isPlaying)
            {
                engine_sound.Play();
            }
        }

        //In Speed Booster effect
        if (isInSpeedBoost)
        {
            speedBoosterTimer += Time.deltaTime;
            if(speedBoosterTimer >= speedBoosterTimeLimit)
            {
                topSpeed -= speedBoostAmount;
                speedBoosterTimer = 0f;
                isInSpeedBoost = false;
            }
        }

        //Rounding off the velocity number
        if (-2 <= v.x && v.x <= 2 && !keypressed)
            v.x = 0;
            
        if (-2 <= v.y && v.y <= 2 && !keypressed)
            v.y = 0;
            
        if(v.x == 0 && v.y == 0)
            engine_sound.Stop();

        //Apply the new velocity to the player
        rb.velocity = v;

        UpdateGUIText();
    }

    public void UpdateGUIText()
    {
        GameObject.FindWithTag("score").GetComponent<TMP_Text>().SetText("{0}", GameManager.GetScore());
        GameObject.FindWithTag("time_extender").GetComponent<TMP_Text>().SetText("{0}", numTimeExtender);
        GameObject.FindWithTag("booster").GetComponent<TMP_Text>().SetText("{0}", numSpeedBooster);
        GameObject.FindWithTag("weight").GetComponent<TMP_Text>().SetText("{0}/{1}", currCargoSize, maxCargoSize);
    }

    public void UpdateCargoPickedupMenu() //Renew the cargo picked up UI
    {
        for(int count = 0; count < 12; count++)
        {
            if (count + 1 > cargoes_pickedup.Count) //change colour of indicator to grey
            {
                cargo_indicators[count].GetComponent<SpriteRenderer>().color = new Color(0.5f, 0.5f, 0.5f);
                continue;
            }
            cargo_indicators[count].GetComponent<SpriteRenderer>().color = GameManager.colours[cargoes_pickedup[count] - 1]; //Load the colour
        }
        UpdateGUIText();
    }

    public static void Upgrade_VehicleCapacity()
    {
        maxCargoSize += vehicleCapacityUpgradeAmount;
    }

    //Vehicle collide with other objects
    public void OnTriggerEnter2D(Collider2D obj)
    {
        string name = obj.gameObject.name;
        if(name.StartsWith("Cargo_")) //handle collision with cargo
        {
            string[] str = name.Split('_');
            SpawnerScript spawner = obj.gameObject.GetComponent<SpawnerScript>();
            if(spawner.isActive) //If the cargo destination is active
            {
                int cargo_num = int.Parse(str[1]);
                if (cargoes_pickedup.Contains(cargo_num)) //If the cargo is picked up and in the truck
                {
                    GameManager.cargoes_active.Remove(cargo_num);
                    cargoes_pickedup.Remove(cargo_num);
                    currCargoSize -= GameManager.size_of_cargoes[cargo_num];
                    UpdateCargoPickedupMenu();
                    spawner.SuccessfulDelivery();
                }
                
            }
        }
        else if(name.StartsWith("CargoDistribution_")) // Player reach cargo distribution center
        {
            if( (GameManager.CDCWestUnlocked && name == "CargoDistribution_West") || name == "CargoDistribution_East" )
            {
                if (!cargo_menu_opened && (Time.time - cargo_dist_exit_time >= 0.5f)) //Not opened menu when within 0.5s of leaving the CDC
                {
                    cargo_dist_exit_time = Time.time; //Record the time the player enters cargo distribution center
                    currTripCargoDelivered = 0; //Reset cargo delivered counter
                    GameObject canvas = GameObject.Find("Canvas");
                    cargo_menu_opened = true;
                    canvas.GetComponent<CargoPickupMenuScript>().OpenMenu();
                }
            }
        }
        else if( (name == "AI" || name == "AI(Clone)") && obj is PolygonCollider2D && capCol.IsTouching(obj)) //Player hit the AI
        {
            GameManager.ReduceScore(20);
            GameObject.FindWithTag("AudioManager").GetComponent<SoundEffects_Script>().playCarCrash();
            playerCollideWithAI++;
            Destroy(obj.gameObject);
            AI_Respawn.respawnAI();
        }
    }

    public void OnTriggerExit2D(Collider2D obj)
    {
        string name = obj.gameObject.name;
        if (name.StartsWith("CargoDistribution_"))
            cargo_dist_exit_time = Time.time; //Record the time the player leaves cargo distribution center
    }
}
