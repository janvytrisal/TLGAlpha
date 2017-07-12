using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Save end game.
 */
public class InitializeEndGame : MonoBehaviour 
{
    void Start()
    {
        PrepareSaveGame();
        GameManager.SaveAll();
    }

    private void PrepareSaveGame()
    {
        if (GameManager.DeathCount < GameManager.Highscore)
        {
            GameManager.Highscore = GameManager.DeathCount;
        }
        GameManager.DeathCount = int.MaxValue; //not relevant value
        GameManager.Seeds.Clear(); //to not save unwanted data
    }
}
