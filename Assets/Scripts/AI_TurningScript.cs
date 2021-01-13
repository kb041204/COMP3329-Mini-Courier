using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_TurningScript : MonoBehaviour
{
    public int turnNumber;
    public char carOriginalDirection;
    public char turnDirection;
    public char originSide;
    private BoxCollider2D boxColl;

    void Start()
    {
        boxColl = GetComponent<BoxCollider2D>();
    }

    public void OnTriggerEnter2D(Collider2D obj)
    {
        string name = obj.gameObject.name;
        PolygonCollider2D polyCol = obj.GetComponent<PolygonCollider2D>();

        AI_Script ai_script = obj.gameObject.GetComponent<AI_Script>();

        //AI touches the correct turning point 
        if ( (name == "AI" || name == "AI(Clone)") && turnNumber == ai_script.nextTurn && boxColl.IsTouching(polyCol) && ai_script.currDirection == carOriginalDirection && ai_script.originSide == originSide)
        {
            Rigidbody2D rb = obj.gameObject.GetComponent<Rigidbody2D>();

            // Rotate the AI and change its speed according to their turn direction
            if (turnDirection == 'e')
            {
                obj.gameObject.transform.eulerAngles = new Vector3(0, 0, 0);
                rb.velocity = new Vector2(ai_script.maxSpeed, 0);
            }
            else if (turnDirection == 's')
            {
                obj.gameObject.transform.eulerAngles = new Vector3(0, 0, 270);
                rb.velocity = new Vector2(0, -ai_script.maxSpeed);
            }
            else if (turnDirection == 'w')
            {
                obj.gameObject.transform.eulerAngles = new Vector3(0, 180, 0);
                rb.velocity = new Vector2(-ai_script.maxSpeed, 0);
            }
            else if (turnDirection == 'n')
            {
                obj.gameObject.transform.eulerAngles = new Vector3(0, 0, 90);
                rb.velocity = new Vector2(0, ai_script.maxSpeed);
            }

            ai_script.currDirection = turnDirection; //Update AI direction
        }
    }
}
