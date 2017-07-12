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
        RectTransform rectTransform = healthPrefab.GetComponent<RectTransform>();
        for (int i = 0; i < _playerHealth.CurrentHealth; i++)
        {
            _healthBar.Add(Instantiate(healthPrefab));
            _healthBar[i].transform.SetParent(transform);
            _healthBar[i].GetComponent<RectTransform>().anchoredPosition = (Vector2.right * ((rectTransform.sizeDelta.x / 2) - 10f) * i);
        }
    }
}
