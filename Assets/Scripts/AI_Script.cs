using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_Script : MonoBehaviour
{
    public int nextTurn;
    public float maxSpeed;
    private float speed;

    public char initialDirection;
    public char currDirection;
    public char originSide;

    private CircleCollider2D cirCol;
    private PolygonCollider2D polyCol;
    private BoxCollider2D boxCol;
    private Rigidbody2D rb;

    // Assume initial direction only East or West
    void Start()
    {
        speed = maxSpeed;
        rb = GetComponent<Rigidbody2D>();
        currDirection = initialDirection;
        cirCol = GetComponent<CircleCollider2D>();
        polyCol = GetComponent<PolygonCollider2D>();
        boxCol = GetComponent<BoxCollider2D>();

        //Change the rotation of the AI
        if (initialDirection == 'e')
        {
            gameObject.transform.eulerAngles = new Vector3(0, 0, 0); //Rotate
            rb.velocity = new Vector2(speed, 0); //Give velocity
        }
        else if (initialDirection == 's')
        {
            gameObject.transform.eulerAngles = new Vector3(0, 0, 270);
            rb.velocity = new Vector2(0, -speed);
        }
        else if (initialDirection == 'w')
        {
            gameObject.transform.eulerAngles = new Vector3(0, 180, 0);
            rb.velocity = new Vector2(-speed, 0);
        }
        else if (initialDirection == 'n')
        {
            gameObject.transform.eulerAngles = new Vector3(0, 0, 90);
            rb.velocity = new Vector2(0, speed);
        }
    }
    
    void Update() //If AI is in the wrong direction, update velocity back to normal
    {
        if (currDirection == 'e' && rb.velocity.x < 0)
            rb.velocity = new Vector2(speed, 0);
        else if (currDirection == 's' && rb.velocity.y > 0)
            rb.velocity = new Vector2(0, -speed);
        else if (currDirection == 'w' && rb.velocity.x > 0)
            rb.velocity = new Vector2(-speed, 0);
        else if (currDirection == 'n' && rb.velocity.y < 0)
            rb.velocity = new Vector2(0, speed);
    }
    
    void changeSpeed(float _speed) //Resume speed
    {
        speed = _speed;
        if (currDirection == 'e')
            rb.velocity = new Vector2(_speed, 0);
        else if (currDirection == 's')
            rb.velocity = new Vector2(0, -_speed);
        else if (currDirection == 'w')
            rb.velocity = new Vector2(-_speed, 0);
        else if (currDirection == 'n')
            rb.velocity = new Vector2(0, _speed);
    }

    public void OnTriggerEnter2D(Collider2D obj)
    {
        string name = obj.gameObject.name;
        if (name == "AI" || name == "AI(Clone)") //AI collide with AI
        {
            if (cirCol.IsTouching(obj)) //AI is close to another AI
            {
                changeSpeed(obj.gameObject.GetComponent<AI_Script>().speed); //AI reduce speed to not crash into the front AI
            }
            else if (boxCol.IsTouching(obj) && obj is BoxCollider2D) //AI physically touches another AI
            {
                Destroy(obj.gameObject); //Destroy the AI to prevent it from stopping
                AI_Respawn.respawnAI();
            }
        }
        
        else if(name == "vehicle" && boxCol.IsTouching(obj) && obj is BoxCollider2D) //AI hit the player
        {
            Destroy(gameObject); //Destroy itself to prevent it from stopping
            AI_Respawn.respawnAI();
        }
        
    }
    public void OnTriggerExit2D(Collider2D obj)
    {
        string name = obj.gameObject.name;
        if (name == "AI" || name == "AI(Clone)") //AI is no longer touching another AI
        {
            changeSpeed(obj.gameObject.GetComponent<AI_Script>().maxSpeed); //Resume back to its normal speed
        }
    }
}
