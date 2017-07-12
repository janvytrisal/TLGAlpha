using UnityEngine;
using System.Collections;

public class PlayerHealth : MonoBehaviour 
{
    private float _currentHealth;

    public float maxHealth;

    public float CurrentHealth
    {
        get
        {
            return _currentHealth;
        }
        set
        {
            _currentHealth = value;
        }
    }

    void Awake()
    {
        _currentHealth = maxHealth;        
    }
}
