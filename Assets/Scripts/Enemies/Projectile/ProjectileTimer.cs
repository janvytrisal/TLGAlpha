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
 * Deactive projectile after certain period of time.
 */
public class ProjectileTimer : MonoBehaviour 
{
    private float _timePassed;

    public float deletionTime;

    void Start()
    {
        _timePassed = 0;
    }
    void Update()
    {
        if (_timePassed < deletionTime)
        {
            _timePassed += Time.deltaTime;
        }
        else
        {
            DeactivateProjectile();
        }
    }

    public void DeactivateProjectile()
    {
        transform.position = ProjectileSpawner.defaultProjectilePoolLocation;
        _timePassed = 0;
        gameObject.SetActive(false);
    }
}
