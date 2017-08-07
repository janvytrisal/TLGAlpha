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
using System.Collections.Generic;

/*
 * Loads and Updates health bar HUD when player has different health total from last frame.
 */
public class DisplayHealthBar : MonoBehaviour 
{
    private PlayerHealth _playerHealth;
    private float _lastFrameHealth;
    private List<GameObject> _healthBar;

    public GameObject healthPrefab; //picture

    void Start()
    {
        _playerHealth = GameManager.Player.GetComponent<PlayerHealth>();
        LoadHealthBar();
    }

    void Update()
    {
        _playerHealth = GameManager.Player.GetComponent<PlayerHealth>(); //cut this

        if (_playerHealth.CurrentHealth != _lastFrameHealth)
        {
            UpdateHealthBar();
        }
        _lastFrameHealth = _playerHealth.CurrentHealth;
    }

    private void UpdateHealthBar()
    {
        float healthDifference = _lastFrameHealth - _playerHealth.CurrentHealth;
        if (healthDifference > 0)
        {
            for (int i = (int)_playerHealth.CurrentHealth; i < (int)_lastFrameHealth; i++)
            {
                _healthBar[i].SetActive(false);
            }
        }
        else if(healthDifference < 0)
        {
            for (int i = (int)_lastFrameHealth; i < (int)_playerHealth.CurrentHealth; i++)
            {
                _healthBar[i].SetActive(true);
            }
        }
    }
    private void LoadHealthBar()
    {
        _healthBar = new List<GameObject>();
        for (int i = 0; i < _playerHealth.CurrentHealth; i++)
        {
            _healthBar.Add(Instantiate(healthPrefab));
            RectTransform rect = _healthBar[i].GetComponent<RectTransform>();
            _healthBar[i].transform.SetParent(transform);
            float offset = 40 * i;
            rect.offsetMin = new Vector2(offset, 0); //left, bottom
            rect.offsetMax = new Vector2(offset, 0); //right, top
            rect.localScale = Vector3.one;
            //_healthBar[i].GetComponent<RectTransform>().anchoredPosition = (Vector2.right * ((rectPrefab.sizeDelta.x / 2) - 10f) * i);
        }
    }
}
