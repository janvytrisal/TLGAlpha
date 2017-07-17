/*
 * TLG Alpha
 * Copyright (C) 2017 Jan Vytrisal
 * 
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, version 3 of the License only.
 * 
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with this program.  If not, see <http://www.gnu.org/licenses/>
 */

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
