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
