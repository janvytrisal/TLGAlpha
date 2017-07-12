using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Display actual level.
 */ 
public class SignLevelDisplay : MonoBehaviour 
{
	private TextMesh _textMesh;

	void Start()
	{
		_textMesh = GetComponent<TextMesh>();
		_textMesh.text = "LEVEL\n" + GameManager.Seeds.Count;
		_textMesh.color = ChoseTextColor();
	}

	private Color ChoseTextColor()
	{
		float actsTotal = LevelSpecifications.acts;
		float currentAct = GetAct();
		float red = currentAct / (actsTotal - 1);
		float green = 2 * (1 - red);
		Color color = new Color(2 * red, green, 0);
		return color;
	}
	//replace it (make LevelSpecifications singleton mby)
	private float GetAct()
	{
		float currentLevel = GameManager.Seeds.Count - 1;
		float levelsPerAct = LevelSpecifications.levels / LevelSpecifications.acts;
		float act = Mathf.Floor(currentLevel / levelsPerAct); //0, 1, 2, ... , 9
		return act;
	}
}
