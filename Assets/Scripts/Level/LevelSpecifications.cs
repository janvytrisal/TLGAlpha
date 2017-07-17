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
 * Specifies level variables. 
 * With increasing currentLevel makes level more difficult.
 */
public class LevelSpecifications : MonoBehaviour 
{
    private float _minPlatforms = 8; //start/end platforms not included
    private float _maxPlatforms = 30;
    private float _minEnemies = 2;
    private float _maxEnemies = 100;
    private float _minFireFrequency = 2;
    private float _maxFireFrequency = 0.5f;

    public static float levels = 50;
    public static float acts = 10;

	//currentLevel: 0, 1, ... , (levels - 1)
    public void GetPlatformCounts(float currentLevel, out float staticPlatforms, out float movingPlatforms)
    {
        float levelDifficulty = GetLevelDifficulty(currentLevel);
        float platformsTotal = (_maxPlatforms - _minPlatforms) * levelDifficulty  + _minPlatforms;
        movingPlatforms = Mathf.Round(platformsTotal * levelDifficulty);
        staticPlatforms = Mathf.Round(platformsTotal - movingPlatforms);
    }
    public float GetEnemyCount(float currentLevel)
    {
        float levelDifficulty = GetLevelDifficulty(currentLevel);
        float enemyCount = (_maxEnemies - _minEnemies) * levelDifficulty + _minEnemies;
        enemyCount = Mathf.Round(enemyCount);
        return enemyCount;
    }
    public float GetFireFrequency(float currentLevel)
    {
        float levelDifficulty = GetLevelDifficulty(currentLevel);
        float fireFrequency = _minFireFrequency - (_minFireFrequency - _maxFireFrequency) * levelDifficulty;
        return fireFrequency;
    }

    //linear act difficulty: //0, 0.5, 1
    private float GetLevelDifficulty(float currentLevel)
    {
        float currentAct = GetAct(currentLevel);
		float actDifficultyMultiplier = GetActDifficultyMultiplier();
		float innerActLevelIndex = GetInnerActLevelIndex(currentLevel);
		float levelDifficultyFactor = GetLevelDifficultyFactor();
		float actDifficulty = (currentAct * actDifficultyMultiplier) + (innerActLevelIndex * levelDifficultyFactor);
        return actDifficulty;
    }
    private float GetAct(float currentLevel)
    {
        float levelsPerAct = levels / acts;
        float act = Mathf.Floor(currentLevel / levelsPerAct); //0, 1, 2, ... , 9
        return act;
    }
    private float GetActDifficultyMultiplier() 
    {
        float difficultyFactor = 1 / acts;
        return difficultyFactor;
	}
	private float GetInnerActLevelIndex(float currentLevel)
	{
		float levelsPerAct = levels / acts;
		float innerActLevelIndex = currentLevel % levelsPerAct;
		return innerActLevelIndex;
	}
	private float GetLevelDifficultyFactor()
	{
		float actDifficultyMultiplier = GetActDifficultyMultiplier();
		float levelsPerAct = levels / acts;
		float levelsPerActPercentage = actDifficultyMultiplier / levelsPerAct;
		float decreasedLevelPerActPercentage = levelsPerActPercentage / 1.25f;
		return decreasedLevelPerActPercentage;
	}
}
