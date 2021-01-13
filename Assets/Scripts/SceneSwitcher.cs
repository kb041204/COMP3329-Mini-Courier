using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour
{
    public void MainMenu ()
    {
        GameObject.FindWithTag("AudioManager").GetComponent<SoundEffects_Script>().playClickButton();
        SceneManager.LoadScene(0);
    }
    public void MapSelection()
    {
        GameObject.FindWithTag("AudioManager").GetComponent<SoundEffects_Script>().playClickButton();
        SceneManager.LoadScene(1);
    }
    public void VehicleSelection()
    {
        GameObject.FindWithTag("AudioManager").GetComponent<SoundEffects_Script>().playClickButton();
        SceneManager.LoadScene(2);
    }

    public void PlayTokyo()
    {
        GameObject.FindWithTag("AudioManager").GetComponent<SoundEffects_Script>().playClickButton();
        SceneManager.LoadScene(3);
    }

    public void PlayHongKong()
    {
        GameObject.FindWithTag("AudioManager").GetComponent<SoundEffects_Script>().playClickButton();
        SceneManager.LoadScene(4);
    }
    public void PlayNewYork()
    {
        GameObject.FindWithTag("AudioManager").GetComponent<SoundEffects_Script>().playClickButton();
        SceneManager.LoadScene(5);
    }
    public void HowToPlay()
    {
        GameObject.FindWithTag("AudioManager").GetComponent<SoundEffects_Script>().playClickButton();
        SceneManager.LoadScene(6);
    }
    public void Credits()
    {
        GameObject.FindWithTag("AudioManager").GetComponent<SoundEffects_Script>().playClickButton();
        SceneManager.LoadScene(7);
    }
    public void QuitGame()
    {
        GameObject.FindWithTag("AudioManager").GetComponent<SoundEffects_Script>().playClickButton();
        Application.Quit();
    }
}
