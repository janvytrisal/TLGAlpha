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
 * Stops fast moving player to jump throught moving platform. Set player's y velocity to zero when player enter the trigger.
 */
public class PlayerStopper : MonoBehaviour 
{
    void OnTriggerEnter(Collider otherCollider)
    {
        if (otherCollider.gameObject.tag == "Player")
        {
            PlayerMotions playerMotions = otherCollider.gameObject.GetComponent<PlayerMotions>();
            Vector3 oldVelocity = playerMotions.Velocity;
            playerMotions.Velocity = new Vector3(oldVelocity.x, 0, oldVelocity.z);
        }
    }
}
