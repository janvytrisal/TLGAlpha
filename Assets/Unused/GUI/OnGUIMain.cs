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
using System.Collections;

/*
 * Displays game data.
 */
public class OnGUIMain : MonoBehaviour 
{
    void OnGUI()
    {
        GUI.Label(new Rect(10, 475, 500, 25), "Level number: " + GameManager.Seeds.Count);
        GUI.Label(new Rect(10, 500, 500, 25), "Level identificator: " + GameManager.Seeds[GameManager.Seeds.Count - 1]);
        GUI.Label(new Rect(10, 575, 500, 25), "Deaths: " + GameManager.DeathCount);
        GUI.Label(new Rect(10, 600, 500, 25), "Highscore: " + GameManager.Highscore);
        GUI.Label(new Rect(10, 625, 500, 200), "Seedlist: " + SeedList());
    }
    private string SeedList()
    {
        string seedList = "";
        foreach(int seed in GameManager.Seeds)
        {
            seedList += seed + ", ";
        }
        return seedList;
	}
}
