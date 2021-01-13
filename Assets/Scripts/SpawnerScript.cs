using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; //For TextMeshPro TMP_Text

public class SpawnerScript : MonoBehaviour
{
    public float timer = 0f; //timer for each delivery
    public bool isActive = false;
    public int cargo_number;
    public int score;
    private float time_limit = 0f;
    private GameObject[] timer_texts = new GameObject[27];
    private GameObject[] size_texts = new GameObject[27];

    // Start is called before the first frame update
    void Start()
    {
        //Part 1: Load all timer_texts and size_texts GameObjects
        GameObject[] temp_timer_texts = GameObject.FindGameObjectsWithTag("cargo_timer");
        foreach (GameObject timer in temp_timer_texts)
        {
            string[] name = timer.name.Split('_');
            timer_texts[int.Parse(name[1])] = timer;
        }

        GameObject[] temp_size_texts = GameObject.FindGameObjectsWithTag("cargo_size");
        foreach (GameObject size in temp_size_texts)
        {
            string[] name = size.name.Split('_');
            size_texts[int.Parse(name[1])] = size;
        }

        //Part 2: Reset all the text of timer_texts and size_texts
        GameObject[] temp_gameobject;
        temp_gameobject = GameObject.FindGameObjectsWithTag("cargo_timer");
        foreach (GameObject spawner in temp_gameobject)
            spawner.GetComponent<TMP_Text>().SetText("");
        temp_gameobject = GameObject.FindGameObjectsWithTag("cargo_size");
        foreach (GameObject spawner in temp_gameobject)
            spawner.GetComponent<TMP_Text>().SetText("");
    }

    // Update is called once per frame
    void Update()
    {
        if(isActive)
        {
            //Part 1: Update timer value
            timer += Time.deltaTime;

            //Part 2: Update timer and size text
            timer_texts[cargo_number].GetComponent<TMP_Text>().SetText("{0}", (time_limit - (int)timer));
            if(score > 1)
               size_texts[cargo_number].GetComponent<TMP_Text>().SetText("{0}", score);
            
            //Part 3: Check times up
            if (timer >= time_limit)
                GameObject.Find("GameManager").GetComponent<GameManager>().GameOver();
        }
    }

    public void InitiateSpawner(int _number, int _score, Color _color)
    {
        cargo_number = _number;
        score = _score;
        time_limit = GameManager.time_limit;
        GetComponent<SpriteRenderer>().color = _color;
        isActive = true;
    }

    // Function called when the player made a successful delivery
    public void SuccessfulDelivery()
    {
        isActive = false;
        int total_score = score + (int)((time_limit - timer) / 15) + VehicleScript.currTripCargoDelivered++; 
        //total score = size + (floor)(time remaining/15) + currTripCargoDelivered

        timer = 0f;
        timer_texts[cargo_number].GetComponent<TMP_Text>().SetText("");
        size_texts[cargo_number].GetComponent<TMP_Text>().SetText("");
        GetComponent<SpriteRenderer>().color = Color.white;
        GameObject.Find("GameManager").GetComponent<GameManager>().AddScore(total_score);
    }

    public void IncreaseTimeLimit(float amount) //for the item time extender
    {
        time_limit += amount;
    }
}