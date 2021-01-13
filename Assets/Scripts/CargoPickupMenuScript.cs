using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro; //For TextMeshPro TMP_Text

public class CargoPickupMenuScript : MonoBehaviour
{
    public GameObject pickupMenuUI;
    private GameObject player;
    private GameObject [] pickupMenu_buttons = new GameObject[27];

    void Start()
    {
        player = GameObject.FindWithTag("Player");
        pickupMenuUI.SetActive(false);
    }

    void reloadCargoPickupMenu()
    {
        for (int i = 1; i < GameManager.num_spawner; i++) 
        {
            pickupMenu_buttons[i].GetComponent<Button>().interactable = false; //disable all buttons at first
            pickupMenu_buttons[i].GetComponent<Image>().color = new Color(0f, 0f, 0f); //set to white
            GameObject.Find(pickupMenu_buttons[i].name + "_text").GetComponent<TMP_Text>().SetText(""); //clear all text
        }

        foreach (int cargo in GameManager.cargoes_waiting_pickup) //Enable buttons which is waiting to be pickup
        {
            if (GameManager.size_of_cargoes[cargo] <= VehicleScript.maxCargoSize - VehicleScript.currCargoSize) //player's vehicle has enough capacity
            {
                pickupMenu_buttons[cargo].GetComponent<Button>().interactable = true; //enable button
                pickupMenu_buttons[cargo].GetComponent<Image>().color = GameManager.colours[cargo - 1]; //change colour to matching colour
                GameObject.Find(pickupMenu_buttons[cargo].name + "_text").GetComponent<TMP_Text>().SetText("{0}", GameManager.size_of_cargoes[cargo]);
            }
        }

        GameObject.FindWithTag("Player").GetComponent<VehicleScript>().UpdateCargoPickedupMenu();
    }

    public void OpenMenu()
    {
        GameObject.FindWithTag("AudioManager").GetComponent<SoundEffects_Script>().playOpenMenu();
        if (VehicleScript.engine_sound.isPlaying)
            VehicleScript.engine_sound.Stop();

        Time.timeScale = 0f;
        pickupMenuUI.SetActive(true);
        for (int i = 1; i < GameManager.num_spawner; i++) //load each cargo buildings
        {
            string target = "Cargo_" + i.ToString() + "_pickup";
            pickupMenu_buttons[i] = GameObject.Find(target);
        }

        for (int j = GameManager.num_spawner; j < 27; j++)  //remaining cargo buildings
        {
            string target = "Cargo_" + j.ToString() + "_pickup";
            pickupMenu_buttons[j] = GameObject.Find(target);
            pickupMenu_buttons[j].GetComponent<Button>().interactable = false; //disable all buttons
            pickupMenu_buttons[j].GetComponent<Image>().color = new Color(0f, 0f, 0f); //set to black
            GameObject.Find(pickupMenu_buttons[j].name + "_text").GetComponent<TMP_Text>().SetText(""); //clear all text
        }

        PauseMenuScript.cargoPickupMenuOpened = true; //Prevent player from opening pause menu since it may clash with the cargo pickup menu
        reloadCargoPickupMenu();
    }

    public void ButtonPressed(Button btn_pressed) //Handle each button press except the exit button
    {
        GameObject.FindWithTag("AudioManager").GetComponent<SoundEffects_Script>().playPickedUpCargo();
        string[] tmp = btn_pressed.name.Split('_');
        int cargo_num = int.Parse(tmp[1]);

        //no need check coz checked above already
        VehicleScript.cargoes_pickedup.Add(cargo_num);
        GameManager.cargoes_waiting_pickup.Remove(cargo_num);

        VehicleScript.currCargoSize += GameManager.size_of_cargoes[cargo_num];

        reloadCargoPickupMenu();
    }

    public void Resume() //go back to the game
    {
        pickupMenuUI.SetActive(false);
        player.GetComponent<VehicleScript>().cargo_menu_opened = false;
        PauseMenuScript.cargoPickupMenuOpened = false;
        Time.timeScale = 1f;
    }
}
