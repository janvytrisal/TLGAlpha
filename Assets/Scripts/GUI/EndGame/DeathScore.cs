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
using UnityEngine.UI;

/*
 * Sets end game text.
 */
public class DeathScore : MonoBehaviour 
{
	private Text _textScript;

	void Awake()
	{
		_textScript = GetComponent<Text>();
		_textScript.text = "You have beat the long game, while dying only <color=red>" +
            GameManager.DeathCount + "</color> times!"; //during the course of the game
        
        if (GameManager.DeathCount < GameManager.Highscore)
        {
            _textScript.text += "\nThis is a <color=green>new record</color> for least deaths per play through.";
        }
	}
}
