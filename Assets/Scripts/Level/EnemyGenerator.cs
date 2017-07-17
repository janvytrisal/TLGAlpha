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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityUtilityLibrary;

/*
 * Generates enemies next to platforms at random positions. 
 * Uses inner and outer placeholders to get availible space at each platform.
 */
public class EnemyGenerator : MonoBehaviour 
{
    private List<GameObject> _enemies;
    private int _enemyCount;
    private float _fireFrequency;
    private int _maxEnemiesPerPlatform = 3;

    public GameObject enemyPrefab; //public PrefabArray[] enemies;
    public GameObject enemyGathererPrefab;

    public void GenerateEnemies(List<GameObject[]> placeholders, int enemyCount, float fireFrequency)
    {
        _enemyCount = enemyCount;
        _fireFrequency = fireFrequency;
        _enemies = ArrangeEnemies(placeholders);
        SetParentAll(_enemies, enemyGathererPrefab);
    }

    private List<GameObject> ArrangeEnemies(List<GameObject[]> placeholders)
    {
        List<GameObject> enemies = new List<GameObject>();
        List<Quaternion> enemyRotations;
        List<Vector3> enemyPositions = SelectEnemyPositions(placeholders, out enemyRotations);
        for(int i = 0; i < enemyPositions.Count; i++)
        {
            GameObject enemy = Instantiate(enemyPrefab, enemyPositions[i], enemyRotations[i]);
            enemy.GetComponent<ProjectileSpawner>().fireFrequency = _fireFrequency;
            enemies.Add(enemy);
        }
        return enemies;
    }
    private List<Vector3> SelectEnemyPositions(List<GameObject[]> placeholders, out List<Quaternion> selectedEnemyRotations)
    {
        List<Vector3> allEnemyPositions = new List<Vector3>();
        List<Quaternion> allEnemyRotations = new List<Quaternion>();
        selectedEnemyRotations = new List<Quaternion>();
        for (int i = 5; i < placeholders.Count - 1; i++)
        {
            List<Vector3> enemyPositions = CreateEnemyPositions(placeholders[i][0], placeholders[i][1]);
            allEnemyPositions.AddRange(enemyPositions);
            List<Quaternion> enemyRotations = CreateEnemyRotations(enemyPositions.Count, placeholders[i][0]);
            allEnemyRotations.AddRange(enemyRotations);
        }
        List<Vector3> selectedEnemyPositions = new List<Vector3>();
        while ((selectedEnemyPositions.Count < _enemyCount) && (allEnemyPositions.Count > 0))
        {
            int enemyIndex = Random.Range(0, allEnemyPositions.Count);

            selectedEnemyPositions.Add(allEnemyPositions[enemyIndex]);
            allEnemyPositions.RemoveAt(enemyIndex);

            selectedEnemyRotations.Add(allEnemyRotations[enemyIndex]);
            allEnemyRotations.RemoveAt(enemyIndex);
        }
        return selectedEnemyPositions;
    }
    //Assumption: wall.lossyScale.x == 1
    private List<Vector3> CreateEnemyPositions(GameObject innerPlaceholder, GameObject outerPlaceholder)
    {
        List<Vector3> enemyPositions = new List<Vector3>();
        Vector3 innerPosition = innerPlaceholder.transform.position;
        PlaceholderFlag flag = outerPlaceholder.GetComponent<PlaceholderFlag>();
        Vector3 innerScale = innerPlaceholder.transform.lossyScale;
        Vector3 outerScale = outerPlaceholder.transform.lossyScale;
        Vector3 enemyScale = enemyPrefab.transform.lossyScale;
        float wallXScale = 1; //wall.lossyScale.x
        float emptySpaceOffset = 1;
        float xSpace = (outerScale.x / 2) - (innerScale.x / 2) - wallXScale; 
        float zSpace = innerScale.z - (2 * emptySpaceOffset);

        List<float> zOffsets = GetDefaultEnemyZOffsets(zSpace, enemyScale);
        foreach (float zOffset in zOffsets)
        {
            Vector3 enemyOffset = new Vector3(
                                      (innerScale.x / 2) + (xSpace / 2),
                                      (innerScale.y / 2) + enemyScale.y, //enemyScale.y / 2
                                      zOffset
                                  ); 
            if (flag != null)
            {
                if(outerPlaceholder.transform.right == flag.OccupiedSide)
                {
                    enemyOffset.x *= -1; //because enemyOffset is set to right side by default
                }
                Vector3 enemyPosition = innerPosition + (innerPlaceholder.transform.rotation * enemyOffset);
                enemyPositions.Add(enemyPosition);
            }
            else
            {
                Vector3 enemyPosition = innerPosition + (innerPlaceholder.transform.rotation * enemyOffset);
                enemyPositions.Add(enemyPosition);
                enemyOffset.x *= -1;
                enemyPosition = innerPosition + (innerPlaceholder.transform.rotation * enemyOffset);
                enemyPositions.Add(enemyPosition);
            }
        }
        return enemyPositions;
    }
    //Assumptions: platforms are z symetrical, enemy.lossyScale.x < xSpace
    private List<float> GetDefaultEnemyZOffsets(float zSpace, Vector3 enemyScale)
    {
        List<float> enemyZOffsets = new List<float>();
        float emptySpaceOffest = 0.5f;
        float zOffset = 0;
        bool full = false;

        enemyZOffsets.Add(zOffset);
        do
        {
            zOffset += enemyScale.z + emptySpaceOffest;
            if ((zOffset + (enemyScale.z / 2)) < (zSpace / 2))
            {
                enemyZOffsets.Add(zOffset);
                enemyZOffsets.Add(-zOffset);
            }
            else
            {
                full = true;
            }
        }
        while(!full && (enemyZOffsets.Count < _maxEnemiesPerPlatform));
        return enemyZOffsets;
    }
    private List<Quaternion> CreateEnemyRotations(int enemyPositionCount, GameObject placeholder)
    {
        List<Quaternion> enemyRotations = new List<Quaternion>();
        Quaternion rotation = placeholder.transform.rotation;
        while (enemyRotations.Count < enemyPositionCount)
        {
            enemyRotations.Add(rotation);
        }
        return enemyRotations;
    }
    private void SetParentAll(List<GameObject> gameObjects, GameObject parent)
    {
        foreach (GameObject gameObject in gameObjects)
        {
            gameObject.transform.SetParent(parent.transform);
        }
    }
}
