using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu_AI : MonoBehaviour
{
    //This script is responsible for mangaging AIs in the main menu at the ***bottom lane***

    private Rigidbody2D rb;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 v = rb.velocity;
        v.x = -200;
        v.y = 0;
        rb.velocity = v;
        transform.eulerAngles = new Vector3(0, 180, 0);
    }

    public void OnTriggerEnter2D(Collider2D obj)
    {
        string name = obj.gameObject.name;
        if (name == "AI_despawn")
        {
            Destroy(gameObject); //Destroy itself
        }
    }
}
