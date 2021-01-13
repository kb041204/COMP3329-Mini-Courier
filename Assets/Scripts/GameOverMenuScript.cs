using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro; //For TextMeshPro TMP_Text

public class GameOverMenuScript : MonoBehaviour
{
    public GameObject gameOverMenuUI;
    public GameObject this_isnt;

    void Start()
    {
        gameOverMenuUI.SetActive(false);
    }

    public void callGameOverMenu()
    {
        if (VehicleScript.engine_sound.isPlaying)
            VehicleScript.engine_sound.Stop();

        GameObject.FindWithTag("AudioManager").GetComponent<SoundEffects_Script>().playGameOver();
        gameOverMenuUI.SetActive(true);
        this_isnt.SetActive(false);
        VehicleScript.arrow.SetActive(false);

        //displaying the score and the respective messages
        int score = GameManager.GetScore();
        string scoring_info = "Your score is: " + score.ToString();

        if (score == 69 || score == 420)
            scoring_info += " (Nice)";

        if (score == -69 || score == -420)
            scoring_info += " (Not good, but Nice)";

        if (VehicleScript.playerCollideWithAI >= 5)
            this_isnt.SetActive(true);

        //a new high score?
        if ( (GameManager.map == "Tokyo" && score > PlayerPrefs.GetInt("highscore_tokyo", 0)) || (GameManager.map == "Hong Kong" && score > PlayerPrefs.GetInt("highscore_hong_kong", 0)) || (GameManager.map == "New York" && score > PlayerPrefs.GetInt("highscore_new_york", 0)) )
        {
            scoring_info += "\nA new highscore!";
        }

        GameObject.Find("GameOverScore").GetComponent<TMP_Text>().SetText(scoring_info);
    }

    public void MainMenu() //go back to the main menu
    {
        GameObject.FindWithTag("AudioManager").GetComponent<SoundEffects_Script>().playClickButton();
        Time.timeScale = 1f;
        this_isnt.SetActive(false);
        GameObject.Find("GameManager").GetComponent<GameManager>().ResetGame();
        GameObject.FindWithTag("Player").GetComponent<VehicleScript>().ResetGame();
        SceneManager.LoadScene(0);
    }

    public void QuitGame()
    {
        GameObject.FindWithTag("AudioManager").GetComponent<SoundEffects_Script>().playClickButton();
        Time.timeScale = 1f;
        Application.Quit();
    }
}
