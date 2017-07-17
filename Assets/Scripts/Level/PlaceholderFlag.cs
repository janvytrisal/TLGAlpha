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
 * OccupiedSide holds transform.Right or -transform.Right vector representing which
 * side of placeholder can't be used when building surroundings or adding enemies.
 */
public class PlaceholderFlag : MonoBehaviour 
{
    private Vector3 _occupiedSide;

    public Vector3 OccupiedSide
    {
        get
        {
            return _occupiedSide;
        }
        set
        {
            _occupiedSide = value;
        }
    }


}
