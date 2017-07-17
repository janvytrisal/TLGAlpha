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
 * Firing fireworks after short period of time in endless Update loop.
 */
public class Celebration : MonoBehaviour 
{
    private float _elapsedTime;
    private float _fireDelay;

    public GameObject firework;
    public float fireDelayMin;
    public float fireDelayMax;

    void Start()
    {
        _elapsedTime = 0;
        _fireDelay = GetRandomFireDelay();
    }
    void Update()
    {
        _elapsedTime += Time.deltaTime;
        if (_elapsedTime >= _fireDelay)
        {
            FireAFirework();
            _elapsedTime = 0;
            _fireDelay = GetRandomFireDelay();
        }
    }

    private float GetRandomFireDelay()
    {
        float delay = (Random.value * (fireDelayMax - fireDelayMin)) + fireDelayMin;
        return delay;
    }
    private void FireAFirework()
    {
        Vector3 position = GetRandomPosition();
        Instantiate(firework, position, Quaternion.identity);
    }
    private Vector3 GetRandomPosition()
    {
        int width = Camera.main.pixelWidth;
        int height = Camera.main.pixelHeight;
        int offset = height / 10;

        int x = Random.Range(offset, (width - offset));
        int y = Random.Range(offset, (height - offset));
        int z = Random.Range(15, 50);
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(new Vector3(x, y, z));
        return worldPosition;
    } 
}
