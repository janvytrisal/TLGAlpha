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
