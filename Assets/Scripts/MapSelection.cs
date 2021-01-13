using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MapSelection : MonoBehaviour
{
    public void chooseTokyo()
    {
        GameObject.FindWithTag("AudioManager").GetComponent<SoundEffects_Script>().playClickButton();
        GameManager.map = "Tokyo";
        GameManager.num_spawner = 26;
        SceneManager.LoadScene(2);
    }

    public void chooseHongKong()
    {
        GameObject.FindWithTag("AudioManager").GetComponent<SoundEffects_Script>().playClickButton();
        GameManager.map = "Hong Kong";
        GameManager.num_spawner = 24;
        SceneManager.LoadScene(2);
    }

    public void chooseNewYork()
    {
        GameObject.FindWithTag("AudioManager").GetComponent<SoundEffects_Script>().playClickButton();
        GameManager.map = "New York";
        GameManager.num_spawner = 26;
        SceneManager.LoadScene(2);
    }
}