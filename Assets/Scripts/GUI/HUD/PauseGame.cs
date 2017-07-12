using UnityEngine;
using System.Collections;

/*
 * Pause game, switch to pause menu, freeze player motions.
 */
public class PauseGame : MonoBehaviour 
{
    public void PauseOnClick()
    {
        Time.timeScale = 0f;
        gameObject.SetActive(false); //make pause button invisible
        GameManager.Player.GetComponent<PlayerMotions>().enabled = false;
        GameManager.Player.GetComponent<AccelerometerPlayerCamera>().enabled = false; //maybe not neccessary
        GameObject.Find("HUDCanvas").transform.Find("MainPanel").Find("RightHandPanel").Find("JumpPanel").gameObject.SetActive(false);
        GameObject.Find("HUDCanvas").transform.Find("PausePanel").gameObject.SetActive(true);
    }
}
