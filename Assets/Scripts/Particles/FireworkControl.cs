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
 * Firework using particle system. 
 * Is destroyed when stopped playing.
 */
public class FireworkControl : MonoBehaviour 
{
    private ParticleSystem _particleSystem;
    private ParticleSystem.MainModule _mainModule;

    void Start()
    {
        _particleSystem = GetComponent<ParticleSystem>();
        //_particleSystem.startColor
        _mainModule = _particleSystem.main;
        _mainModule.startColor = GetRandomColor();// _colorModule.color;
        _particleSystem.Play();
    }
    void Update()
    {
        if (!_particleSystem.isPlaying)
        {
            Destroy(gameObject);
        }
    }

    private Color GetRandomColor()
    {
        Color randomColor = new Color(0, 0, 0);
        for (int i = 0; i < 2; i++)
        {
            int channel = Random.Range(0, 3);
            float intensity = 0.75f + (0.25f * Random.value);
            randomColor[channel] = intensity;
        }
        return randomColor;
    }
}
