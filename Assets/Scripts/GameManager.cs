using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; //For TextMeshPro TMP_Text
public class GameManager : MonoBehaviour
{
    public static string map;
    public static int num_spawner;
    public static string vehicle;
    private static int score = 0;

    private float spawnInterval = 9f; //initial spawn Interval In Second
    private float spawnIntervalChange = 0.925f; //initial spawn Interval change in percentage
    public static float time_limit = 60f;
    private static int levels = 1; //Current level, level affects spawn interval
    private int scoreMeasuredForUpgrade = 0;
    private float timer = 0f;
    private bool firstCargoSpawned = false;
    public static bool CDCWestUnlocked = false;

    public Sprite truck;
    public Sprite van;
    public Sprite motorcycle;

    private GameObject [] spawners = new GameObject[27]; //spawner[0] is not used, max 26 spawners in the map

    private System.Random ran = new System.Random();

    public static List<int> cargoes_active = new List<int>();
    public static List<int> cargoes_waiting_pickup = new List<int>();
    public static int[] size_of_cargoes = new int[27];

    private TMP_Text highscore;

    //Pre-defined list of colours
    public static Color [] colours = {new Color(165/(float)255,42/(float)255,42/(float)255), new Color(184/(float)255,134/(float)255,11/(float)255), new Color(154/(float)255,205/(float)255,50/(float)255), new Color(47/(float)255,79/(float)255,79/(float)255), new Color(70/(float)255,130/(float)255,180/(float)255), new Color(255/(float)255,0/(float)255,0/(float)255),
        new Color(238/(float)255,232/(float)255,170/(float)255), new Color(124/(float)255,252/(float)255,0/(float)255), new Color(0/(float)255,128/(float)255,128/(float)255), new Color(100/(float)255,149/(float)255,237/(float)255), new Color(255/(float)255,127/(float)255,80/(float)255), new Color(189/(float)255,183/(float)255,107/(float)255),
        new Color(0/(float)255,128/(float)255,0/(float)255), new Color(0/(float)255,255/(float)255,255/(float)255), new Color(30/(float)255,144/(float)255,255/(float)255), new Color(250/(float)255,128/(float)255,114/(float)255), new Color(128/(float)255,128/(float)255,0/(float)255), new Color(0/(float)255,250/(float)255,154/(float)255),
        new Color(72/(float)255,209/(float)255,204/(float)255), new Color(25/(float)255,25/(float)255,112/(float)255), new Color(255/(float)255,140/(float)255,0/(float)255), new Color(75/(float)255,0/(float)255,130/(float)255), new Color(255/(float)255,255/(float)255,0/(float)255), new Color(46/(float)255,139/(float)255,87/(float)255), 
        new Color(127/(float)255,255/(float)255,212/(float)255), new Color(138/(float)255,43/(float)255,226/(float)255)};

    public void ResetGame() //called when a player go back to the main menu
    {
        score = 0;
        spawnInterval = 9f;
        spawnIntervalChange = 0.925f;
        levels = 1;
        scoreMeasuredForUpgrade = 0;
        timer = 0f;
        CDCWestUnlocked = false;
        for (int i = 0; i < 27; i++)
            size_of_cargoes[i] = 0;
        cargoes_active.Clear();
        cargoes_waiting_pickup.Clear();
    }

    void RefreshHighScore ()
    {
        if (map == "Tokyo")
        {
            highscore.SetText("{0}", PlayerPrefs.GetInt("highscore_tokyo", 0));
        }
        else if (map == "Hong Kong")
        {
            highscore.SetText("{0}", PlayerPrefs.GetInt("highscore_hong_kong", 0));
        }
        else if (map == "New York")
        {
            highscore.SetText("{0}", PlayerPrefs.GetInt("highscore_new_york", 0));
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < 27; i++)
            size_of_cargoes[i] = 0;

        num_spawner += 1;

        firstCargoSpawned = false;

        //Part 1: Change vehicle based on player's choice
        GameObject player = GameObject.FindWithTag("Player");
        SpriteRenderer player_sprite_ren = player.GetComponent<SpriteRenderer>();
        PolygonCollider2D player_poly_coll = player.GetComponent<PolygonCollider2D>();
        
        //---Change sprite
        if (vehicle == "Truck")
            player_sprite_ren.sprite = truck;
        else if (vehicle == "Van")
            player_sprite_ren.sprite = van;
        else if (vehicle == "Motorcycle")
            player_sprite_ren.sprite = motorcycle;

        //---Reset PolygonCollider2D
        Destroy(player_poly_coll);
        player.AddComponent<PolygonCollider2D>();

        //Part 2: Read all the cargo spawner
        GameObject[] temp_spawners = GameObject.FindGameObjectsWithTag("cargo");
        foreach(GameObject spawner in temp_spawners)
        {
            string[] name = spawner.name.Split('_');
            spawners[int.Parse(name[1])] = spawner;
        }

        highscore = GameObject.FindWithTag("highscore").GetComponent<TMP_Text>();
        //Part 3: Refreash high score
        RefreshHighScore();

        //Part 4: Update time limit for each map
        if (map == "Tokyo")
            time_limit = 60;
        else if (map == "Hong Kong")
            time_limit = 75;
        else if (map == "New York")
            time_limit = 75;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        //Check timer and spawn cargoes
        if(timer >= spawnInterval || !firstCargoSpawned) //Skip spawn time interval for first cargo
        {
            firstCargoSpawned = true;
            bool spawned = false;
            while (!spawned)
            {
                int target_spawner_num = ran.Next(1, num_spawner); //choose building for spawning the cargo
                if (!cargoes_active.Contains(target_spawner_num)) //already have cargo spawned? yes -> redraw number
                {
                    int lucky_draw_num = ran.Next(0, 100);
                    int lower_bound = 100 - levels*4; //4% to spawn (n/4+1)-cargo for every n level, remaining = 1 cargo
                    if (lower_bound < 0)
                        lower_bound = 0;
                    int upper_bound = 99;

                    int size;
                    for(size = levels/4 + 1; size > 1; size--) //Check for size of caregoes
                    {
                        if (lower_bound <= lucky_draw_num && lucky_draw_num <= upper_bound)
                            break;
                        lower_bound -= levels*4;
                        upper_bound -= levels*4;
                    }

                    if (size >= VehicleScript.maxCargoSize) //it is possible for size chosen to be larger than truck's max size
                        size = VehicleScript.maxCargoSize;

                    //Debug.Log("Spawned: " + target_spawner_num.ToString());
                    spawners[target_spawner_num].GetComponent<SpawnerScript>().InitiateSpawner(target_spawner_num, size, colours[target_spawner_num - 1]); //Spawn
                    size_of_cargoes[target_spawner_num] = size; //Allocate size into array

                    cargoes_active.Add(target_spawner_num); //Push to List
                    cargoes_waiting_pickup.Add(target_spawner_num);
                    spawned = true;
                    timer = 0;
                }
                else if (cargoes_active.Count == num_spawner) //These lines should never be reached unless the player performs really good
                {
                    spawned = true;
                    timer = 0; //Skip spawning new cargoes
                }
            }
        }
    }

    public void AddScore(int _score)
    {
        score += _score;
        scoreMeasuredForUpgrade += _score;

        GameObject.FindWithTag("AudioManager").GetComponent<SoundEffects_Script>().playDeliveredCargo();

        while (scoreMeasuredForUpgrade >= 20) //player is eligible for upgrades
        {
            levels += 1;
            spawnIntervalChange += (int)levels / 15 * 0.01f; //Reduce the reduce in spawn interval for larger levels
            if (spawnIntervalChange > 1)
                spawnIntervalChange = 1;
            spawnInterval *= spawnIntervalChange; //Reduce spawn interval
            GameObject.FindWithTag("Player").GetComponent<VehicleScript>().UpdateGUIText(); //Update GUI text
            GameObject.Find("Canvas").GetComponent<UpgradeMenuScript>().CallUpgradeMenu(); //Shown upgrade menu
            scoreMeasuredForUpgrade -= 20;
        }
    }

    public static void ReduceScore(int _score)
    {
        score -= _score;
    }

    public static int GetScore()
    {
        return score;
    }

    public static int GetLevel()
    {
        return levels;
    }

    public void GameOver()
    {
        for (int curr_num = 1; curr_num < num_spawner; curr_num++) //Set all timer to be not active so as to invoke GameOver() once
            spawners[curr_num].GetComponent<SpawnerScript>().isActive = false;

        Time.timeScale = 0f; //Freeze the game
        GameObject.Find("Canvas").GetComponent<GameOverMenuScript>().callGameOverMenu(); //Shown game over menu

        //Update highscore if necessary
        if (map == "Tokyo")
        {
            if (score > PlayerPrefs.GetInt("highscore_tokyo", 0))
            {
                PlayerPrefs.SetInt("highscore_tokyo", score);
                RefreshHighScore();
            }
        }
        else if (map == "Hong Kong")
        {
            if (score > PlayerPrefs.GetInt("highscore_hong_kong", 0))
            {
                PlayerPrefs.SetInt("highscore_hong_kong", score);
                RefreshHighScore();
            }
        }
        else if (map == "New York")
        {
            if (score > PlayerPrefs.GetInt("highscore_new_york", 0))
            {
                PlayerPrefs.SetInt("highscore_new_york", score);
                RefreshHighScore();
            }
        }
    }
}
