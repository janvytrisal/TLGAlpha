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
