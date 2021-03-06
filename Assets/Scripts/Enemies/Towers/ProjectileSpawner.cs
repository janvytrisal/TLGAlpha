﻿/*
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
using System.Collections.Generic;

/*
 * Fire projectiles when time is up and view is clear. Uses object pooling.
 */
public class ProjectileSpawner : MonoBehaviour 
{
    private PlayerAim _PlayerAim;
    private float _timeElapsed;
    private GameObject[] _projectilePool;

    public float fireFrequency; //delay in seconds between firing projectiles
    public Transform spawnPoint;
    public GameObject projectilePrefab;

    public static Vector3 defaultProjectilePoolLocation = new Vector3(0, -9999, 0);

    void Start()
    {
        _PlayerAim = GetComponent<PlayerAim>();
        _timeElapsed = fireFrequency;
        InitializeProjectilePool();
    }
    void Update()
    {
        if (_PlayerAim.ClearView) //can fire
        {
            if (_timeElapsed < fireFrequency)
            {
                _timeElapsed += Time.deltaTime;
            }
            else
            {
                if (Fire())
                {
                    _timeElapsed = 0;
                }
            }
        }
    }

    private void InitializeProjectilePool()
    {
        int length = CalculateProjectilePoolLength();
        _projectilePool = new GameObject[length];
        for (int i = 0; i < length; i++)
        {
            _projectilePool[i] = (GameObject)Instantiate(projectilePrefab, defaultProjectilePoolLocation, Quaternion.identity);
            _projectilePool[i].SetActive(false);
        }
    }
    private int CalculateProjectilePoolLength()
    {
        float deletionTime = projectilePrefab.GetComponent<ProjectileTimer>().deletionTime;
        float length = deletionTime / fireFrequency;
        int lengthRounded = Mathf.CeilToInt(length);
        return lengthRounded;
    }
    private bool Fire()
    {
        for (int i = 0; i < _projectilePool.Length; i++)
        {
            if (!_projectilePool[i].activeSelf)
            {
                _projectilePool[i].transform.position = spawnPoint.position; 
                _projectilePool[i].transform.rotation = transform.rotation;
                _projectilePool[i].GetComponent<ProjectileMotion>().Direction = transform.forward;
                _projectilePool[i].SetActive(true);
                return true;
            }
        }
        return false; //all projectiles are in use
    }
}
