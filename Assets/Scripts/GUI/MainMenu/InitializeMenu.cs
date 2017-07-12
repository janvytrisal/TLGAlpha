using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

/*
 * Main menu initialization dependent on game state.
 */
public class InitializeMenu : MonoBehaviour 
{
    void Start()
    {
        if (GameManager.State == ApplicationState.Started)
        {
            GameManager.LoadAll();
        }
        if (GameManager.Seeds.Count > 0)
        {
            GameObject continueButton = GameObject.Find("MainMenuCanvas/MainPanel").transform.
                Find("ContinueButton").gameObject;
            continueButton.SetActive(true);

            int totalSeeds = GameManager.Seeds.Count;

            continueButton.GetComponentInChildren<Text>().
            text = "CONTINUE: level " + totalSeeds + " of " + (int)LevelSpecifications.levels;
        }
        if (GameManager.Highscore < int.MaxValue)
        {
            GameObject highscorePanel = GameObject.Find("MainMenuCanvas").transform.Find("HighscorePanel").gameObject;
            highscorePanel.SetActive(true);
            GameObject text = highscorePanel.transform.Find("Text").gameObject;
            text.GetComponent<Text>().text = GameManager.Highscore.ToString();
        }
    }
}
