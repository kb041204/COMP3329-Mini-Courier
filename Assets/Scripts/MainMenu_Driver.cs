using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu_Driver : MonoBehaviour
{
    private Rigidbody2D rb;
    private char currDirection;

    //This script is responsible for mangaging AIs in the main menu at the ***top lane***

    // Start is called before the first frame update
    void Start()
    {
        currDirection = 'e';
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        float speed = MainMenu_animation.speed;
        Vector2 v = rb.velocity;

        //chane the AI velocity based on their current direction
        if (currDirection == 'e')
        {
            v.x = speed;
            v.y = 0;

        }
        else if (currDirection == 's')
        {
            v.x = 0;
            v.y = -speed;
        }
        else if (currDirection == 'n')
        {
            v.x = 0;
            v.y = speed;
        }

        rb.velocity = v;
    }

    public void OnTriggerEnter2D(Collider2D obj)
    {
        string name = obj.gameObject.name;

        //AI touches the turning point
        if (name == "n")
        {
            transform.eulerAngles = new Vector3(0, 0, 90); //rotate the AI
            currDirection = 'n';
        }
        else if (name == "e")
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
            currDirection = 'e';
        }
        else if (name == "s")
        {
            transform.eulerAngles = new Vector3(0, 0, 270);
            currDirection = 's';
        }
        else if (name == "target") //AI touches the target building
        {
            GameObject.FindWithTag("AudioManager").GetComponent<SoundEffects_Script>().playDeliveredCargo(); //play sound
            obj.gameObject.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f); //change to white color
        }
        else if (name == "truck_despawn") //AI touches the despawn point
        {
            MainMenu_animation.spawned = false;
            Destroy(gameObject); //Destroy itself
        }
    }
}
