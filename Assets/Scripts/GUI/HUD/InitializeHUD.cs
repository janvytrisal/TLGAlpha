﻿/*
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
