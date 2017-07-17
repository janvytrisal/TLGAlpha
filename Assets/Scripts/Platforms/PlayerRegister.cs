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
 * Register player at platform when enter its top trigger.
 * Requeried for MoveAll method in PlatformMotion.
 */
public class PlayerRegister : MonoBehaviour 
{
    private PlatformMotion _parent; //polymorphism

    void Start()
    {
        _parent = GetComponentInParent<PlatformMotion>();
    }

    void OnTriggerEnter(Collider otherCollider)
    {
        if (otherCollider.gameObject.tag == "Player")
        {
            _parent.Player = otherCollider.gameObject;
        }
    }

    void OnTriggerExit(Collider otherCollider)
    {
        if (otherCollider.gameObject.tag == "Player")
        {
            _parent.Player = null;
        }
    }
}
