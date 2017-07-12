using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Show current number of deaths on HUD, if player finished game before.
 */
public class InitializeHUD : MonoBehaviour 
{
    void Awake()
    {
        if (GameManager.Highscore < int.MaxValue) //replace
        {
            GameObject.Find("HUDCanvas/TopPanel").transform.Find("HighscorePanel").gameObject.SetActive(true);
        }
    }
}
