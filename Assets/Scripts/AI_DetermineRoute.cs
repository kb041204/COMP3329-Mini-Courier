using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_DetermineRoute : MonoBehaviour
{
    public int numberOfPossibleRoute;
    public char side;
    private System.Random ran = new System.Random();

    public void OnTriggerEnter2D(Collider2D obj)
    {
        string name = obj.gameObject.name;
        if(name == "AI" || name == "AI(Clone)") //AI touches the turning point
        {
            AI_Script ai_script = obj.gameObject.GetComponent<AI_Script>();
            ai_script.originSide = side;
            ai_script.nextTurn = ran.Next(1, numberOfPossibleRoute + 1); //Generate a random number to determine the AI turning point
        }
    }
}