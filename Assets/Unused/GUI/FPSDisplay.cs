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

public class FPSDisplay : MonoBehaviour 
{
    private float _timeElapsed;
    private float _fpsCounter;
    private float _fpsToDisplay;

    void Start()
    {
        _timeElapsed = 0;
        _fpsCounter = 0;
    }

    void Update()
    {
        _timeElapsed += Time.deltaTime;
        _fpsCounter++;
        if (_timeElapsed >= 1)
        {
            _fpsToDisplay = _fpsCounter;
            _timeElapsed = 0;
            _fpsCounter = 0;
        }
    }

    void OnGUI()
    {
        GUI.Label(new Rect(10, 450, 500, 25), "FPS: " + _fpsToDisplay);
    }
}
