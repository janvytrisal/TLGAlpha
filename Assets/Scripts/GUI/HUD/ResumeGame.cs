using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

/*
 * Resume game, switch to ingame HUD, unfreeze player.
 */
public class ResumeGame : MonoBehaviour 
{
    public void ResumeOnClick()
    {
        Time.timeScale = 1f;
        GameManager.Player.GetComponent<PlayerMotions>().enabled = true;
        GameManager.Player.GetComponent<AccelerometerPlayerCamera>().enabled = true;
        GameObject.Find("HUDCanvas").transform.Find("PausePanel").gameObject.SetActive(false);
        GameObject.Find("HUDCanvas").transform.Find("MainPanel").Find("RightHandPanel").Find("JumpPanel").gameObject.SetActive(true);
        GameObject.Find("HUDCanvas").transform.Find("TopPanel").Find("PauseButton").gameObject.SetActive(true);
    }
}
