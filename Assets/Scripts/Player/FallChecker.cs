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
 * Publisher: raise PlayerFell event.
 * Checks if player fell.
 */
public class FallChecker : MonoBehaviour 
{
    private float _currentFallDepth;
    private float _lastY;

    public float fallDepth;

    public delegate void PlayerFellHandler();
    public static event PlayerFellHandler PlayerFell;

    void Start()
    {
        _currentFallDepth = 0;
    }
    void FixedUpdate()
    {
        CheckCurrentFallDepth();
    }

    private void CheckCurrentFallDepth()
    {
        float y = transform.position.y;
        if ((y < _lastY) && (!GetComponent<PlayerMotions>().IsGrounded()))
        {
            _currentFallDepth += (_lastY - y);
            if (_currentFallDepth >= fallDepth)
            {
                if (PlayerFell != null)
                {
                    PlayerFell();
                }
            }
        }
        else
        {
            _currentFallDepth = 0;
        }
        _lastY = y;
    }
}
