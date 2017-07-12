using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
 * Sets death score.
 * OPTIMISE -> put it outside of Update (event after death).
 */
public class CurrentDeathScore : MonoBehaviour 
{
    private Text _textScript;

    void Start()
    {
        _textScript = GetComponent<Text>();
    }
    void Update()
    {
        _textScript.text = GameManager.DeathCount.ToString(); 
    }
}
