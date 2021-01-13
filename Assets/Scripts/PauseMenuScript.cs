using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuScript : MonoBehaviour
{
    public static bool gameIsPaused = false;
    public static bool cargoPickupMenuOpened = false;
    public GameObject pauseMenuUI;

    void Start()
    {
        pauseMenuUI.SetActive(false);
        gameIsPaused = false;
        cargoPickupMenuOpened = false;
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape)) //player pressed Esc
        {
            if(!cargoPickupMenuOpened) //other menus not opened
            {
                GameObject.FindWithTag("AudioManager").GetComponent<SoundEffects_Script>().playOpenMenu();
                if (VehicleScript.engine_sound.isPlaying) //stop engine sound if it is playing
                    VehicleScript.engine_sound.Stop();

                if (gameIsPaused)
                    Resume();
                else
                    Pause();
            }
            
        }
    }

    public void Resume()
    {
        GameObject.FindWithTag("AudioManager").GetComponent<SoundEffects_Script>().playClickButton();
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        gameIsPaused = false;
    }

    void Pause()
    {
        VehicleScript.arrow.SetActive(false);
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        gameIsPaused = true;
    }

    public void MainMenu()
    {
        GameObject.FindWithTag("AudioManager").GetComponent<SoundEffects_Script>().playClickButton();
        Time.timeScale = 1;
        gameIsPaused = false;
        cargoPickupMenuOpened = false;
        GameObject.Find("GameManager").GetComponent<GameManager>().ResetGame();
        GameObject.FindWithTag("Player").GetComponent<VehicleScript>().ResetGame();
        SceneManager.LoadScene(0);
    }

    public void QuitGame()
    {
        GameObject.FindWithTag("AudioManager").GetComponent<SoundEffects_Script>().playClickButton();
        Application.Quit();
    }
}
