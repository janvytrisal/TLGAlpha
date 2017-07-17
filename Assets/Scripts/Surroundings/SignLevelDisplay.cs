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
 * Display actual level.
 */ 
public class SignLevelDisplay : MonoBehaviour 
{
	private TextMesh _textMesh;

	void Start()
	{
		_textMesh = GetComponent<TextMesh>();
		_textMesh.text = "LEVEL\n" + GameManager.Seeds.Count;
		_textMesh.color = ChoseTextColor();
	}

	private Color ChoseTextColor()
	{
		float actsTotal = LevelSpecifications.acts;
		float currentAct = GetAct();
		float red = currentAct / (actsTotal - 1);
		float green = 2 * (1 - red);
		Color color = new Color(2 * red, green, 0);
		return color;
	}
	//replace it (make LevelSpecifications singleton mby)
	private float GetAct()
	{
		float currentLevel = GameManager.Seeds.Count - 1;
		float levelsPerAct = LevelSpecifications.levels / LevelSpecifications.acts;
		float act = Mathf.Floor(currentLevel / levelsPerAct); //0, 1, 2, ... , 9
		return act;
	}
}
