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
 * Move projectile in specific direction using force.
 */
public class ProjectileMotion : MonoBehaviour 
{
    private Vector3 _direction;

    public float speed;

    public Vector3 Direction
    {
        set
        {
            _direction = value;
        }
    }

    void OnEnable()
    {
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        Vector3 velocity = speed * _direction;
        GetComponent<Rigidbody>().AddForce(velocity, ForceMode.VelocityChange);
    }
}
