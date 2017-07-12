using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
 * Sets end game text.
 */
public class DeathScore : MonoBehaviour 
{
	private Text _textScript;

	void Awake()
	{
		_textScript = GetComponent<Text>();
		_textScript.text = "You have beat the long game, while dying only <color=red>" +
            GameManager.DeathCount + "</color> times!"; //during the course of the game
        
        if (GameManager.DeathCount < GameManager.Highscore)
        {
            _textScript.text += "\nThis is a <color=green>new record</color> for least deaths per play through.";
        }
	}
}
