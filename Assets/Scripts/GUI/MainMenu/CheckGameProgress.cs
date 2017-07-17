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
 * Pop up message box if player press new gave while he/she have already played before.
 */
public class CheckGameProgress : MonoBehaviour
{
    public void CheckGameProgressOnClick()
    {
        if(GameManager.Seeds.Count > 0)
        {
            GameObject warningPanel = GameObject.Find("MainMenuCanvas").transform.Find("WarningPanel").gameObject;
            warningPanel.SetActive(true);
        }
        else
        {
            GameManager.State = ApplicationState.NewGame;
            SceneManager.LoadScene("LevelInfinity", LoadSceneMode.Single);
        }
    }
}

