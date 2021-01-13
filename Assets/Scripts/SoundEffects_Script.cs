using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffects_Script : MonoBehaviour
{
    public AudioClip clickButton;
    public AudioClip timeExtender;
    public AudioClip booster;

    public AudioClip openMenu;

    public AudioClip carCrash;
    public AudioClip pickedUpCargo;
    public AudioClip deliveredCargo;

    public AudioClip gameOver;

    public void playClickButton()
    {
        AudioSource.PlayClipAtPoint(clickButton, transform.position);
    }

    public void playTimeExtender()
    {
        AudioSource.PlayClipAtPoint(timeExtender, transform.position);
    }

    public void playBooster()
    {
        AudioSource.PlayClipAtPoint(booster, transform.position);
    }

    public void playOpenMenu()
    {
        AudioSource.PlayClipAtPoint(openMenu, transform.position);
    }

    public void playPickedUpCargo()
    {
        AudioSource.PlayClipAtPoint(pickedUpCargo, transform.position);
    }

    public void playDeliveredCargo()
    {
        AudioSource.PlayClipAtPoint(deliveredCargo, transform.position);
    }

    public void playGameOver()
    {
        AudioSource.PlayClipAtPoint(gameOver, transform.position);
    }

    public void playCarCrash()
    {
        AudioSource.PlayClipAtPoint(carCrash, transform.position);
    }
}
