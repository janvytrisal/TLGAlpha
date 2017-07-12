using UnityEngine;
using System.Collections;

public class FPSDisplay : MonoBehaviour 
{
    private float _timeElapsed;
    private float _fpsCounter;
    private float _fpsToDisplay;

    void Start()
    {
        _timeElapsed = 0;
        _fpsCounter = 0;
    }

    void Update()
    {
        _timeElapsed += Time.deltaTime;
        _fpsCounter++;
        if (_timeElapsed >= 1)
        {
            _fpsToDisplay = _fpsCounter;
            _timeElapsed = 0;
            _fpsCounter = 0;
        }
    }

    void OnGUI()
    {
        GUI.Label(new Rect(10, 450, 500, 25), "FPS: " + _fpsToDisplay);
    }
}
