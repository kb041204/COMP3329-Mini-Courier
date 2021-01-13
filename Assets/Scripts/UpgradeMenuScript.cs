using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; //For Image component
using TMPro; //For TextMeshPro TMP_Text

public class UpgradeMenuScript : MonoBehaviour
{
    public Sprite upgrade_booster;
    public Sprite upgrade_capacity;
    public Sprite upgrade_CDC;
    public Sprite upgrade_timeExtender;
    public GameObject upgradeMenu;

    private VehicleScript player_script;
    private System.Random ran = new System.Random();
    private GameObject btn_1;
    private GameObject btn_2;
    private TMP_Text btn_1_text;
    private TMP_Text btn_2_text;
    private string btn_1_option;
    private string btn_2_option;

    // Start is called before the first frame update
    void Start()
    {
        upgradeMenu.SetActive(true);
        player_script = GameObject.FindWithTag("Player").GetComponent<VehicleScript>();
        btn_1 = GameObject.Find("Upgrade_button_1");
        btn_2 = GameObject.Find("Upgrade_button_2");
        btn_1_text = GameObject.Find("Upgrade_button_1_text").GetComponent<TMP_Text>();
        btn_2_text = GameObject.Find("Upgrade_button_2_text").GetComponent<TMP_Text>();
        upgradeMenu.SetActive(false);
    }

    void Resume()
    {
        PauseMenuScript.cargoPickupMenuOpened = false;
        Time.timeScale = 1f;
        upgradeMenu.SetActive(false);
    }

    public void CallUpgradeMenu()
    {
        GameObject.FindWithTag("AudioManager").GetComponent<SoundEffects_Script>().playOpenMenu();
        if (VehicleScript.engine_sound.isPlaying)
            VehicleScript.engine_sound.Stop();

        PauseMenuScript.cargoPickupMenuOpened = true; //Prevent player press Esc
        Time.timeScale = 0f;
        upgradeMenu.SetActive(true);

        int lucky_draw = ran.Next(0, 100);
        if(65 <= lucky_draw && lucky_draw <= 99) //35% capacity upgrade
        {
            btn_1.GetComponent<Image>().sprite = upgrade_capacity;
            btn_1_option = "capacity";
            btn_1_text.SetText("Vehicle capacity upgrade");
        }
        else //65% booster
        {
            btn_1.GetComponent<Image>().sprite = upgrade_booster;
            btn_1_option = "booster";
            btn_1_text.SetText("Speed Booster");
        }

        lucky_draw = ran.Next(0, 100);
        if (GameManager.GetLevel() >= 6 && !GameManager.CDCWestUnlocked && 0 <= lucky_draw && lucky_draw <= 30) //After level 6, there are a 30% chance to unlock West CDC
        {
            btn_2.GetComponent<Image>().sprite = upgrade_CDC;
            btn_2_option = "CDC";
            btn_2_text.SetText("Unlock west cargo distribution center");
        }
        else
        {
            if (btn_1_option == "capacity") //If option 1 is capacity, then option 2 will be booster
            {
                btn_2.GetComponent<Image>().sprite = upgrade_booster;
                btn_2_option = "booster";
                btn_2_text.SetText("Speed Booster");
            }
            else if (60 <= lucky_draw && lucky_draw <= 99) //If option 1 is booster, Option 2 will be 40% to be timeExtender
            {
                btn_2.GetComponent<Image>().sprite = upgrade_timeExtender;
                btn_2_option = "timeExtender";
                btn_2_text.SetText("Timer Extender");
            }
            else
            {
                btn_2.GetComponent<Image>().sprite = upgrade_booster;
                btn_2_option = "booster";
                btn_2_text.SetText("Speed Booster");
            }
        }
    }

    public void Button_1_handler()
    {
        GameObject.FindWithTag("AudioManager").GetComponent<SoundEffects_Script>().playClickButton();
        if (btn_1_option == "capacity")
            VehicleScript.Upgrade_VehicleCapacity();
        else
            VehicleScript.numSpeedBooster++;
        Resume();
    }

    public void Button_2_handler()
    {
        GameObject.FindWithTag("AudioManager").GetComponent<SoundEffects_Script>().playClickButton();
        if (btn_2_option == "CDC")
        {
            GameManager.CDCWestUnlocked = true;
            GameObject.Find("CargoDistribution_West").GetComponent<SpriteRenderer>().color = new Color(0f, 0f, 0f); //change west CDC background to black
        }
        else if (btn_2_option == "booster")
            VehicleScript.numSpeedBooster++;
        else if (btn_2_option == "timeExtender")
            VehicleScript.numTimeExtender++;
        Resume();
    }

}
