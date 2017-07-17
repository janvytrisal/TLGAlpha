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
 * Sets death score.
 * OPTIMISE -> put it outside of Update (event after death).
 */
public class CurrentDeathScore : MonoBehaviour 
{
    private Text _textScript;

    void Start()
    {
        _textScript = GetComponent<Text>();
    }
    void Update()
    {
        _textScript.text = GameManager.DeathCount.ToString(); 
    }
}
