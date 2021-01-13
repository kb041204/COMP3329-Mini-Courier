using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VehicleSelection : MonoBehaviour
{

    public void startGame()
    {
        if (GameManager.map == "Tokyo")
            SceneManager.LoadScene(3);
        else if (GameManager.map == "Hong Kong")
            SceneManager.LoadScene(4);
        else if (GameManager.map == "New York")
            SceneManager.LoadScene(5);
    }
    public void chooseMotorcycle()
    {
        GameObject.FindWithTag("AudioManager").GetComponent<SoundEffects_Script>().playClickButton();
        GameManager.vehicle = "Motorcycle";
        startGame();
    }
    public void chooseVan()
    {
        GameObject.FindWithTag("AudioManager").GetComponent<SoundEffects_Script>().playClickButton();
        GameManager.vehicle = "Van";
        startGame();
    }
    public void chooseTruck()
    {
        GameObject.FindWithTag("AudioManager").GetComponent<SoundEffects_Script>().playClickButton();
        GameManager.vehicle = "Truck";
        startGame();
    }
}
