using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityUtilityLibrary;

/*
 * Create walls next to platforms using platforms outer placeholder.
 * Flag restricts where walls can be build.
 * UNFINISHED -> fill gaps, later add floor?
 */
public class SurroundingsGenerator : MonoBehaviour 
{
    public GameObject wallPrefab;
    public GameObject surroundingsGathererPrefab;

    public void GenerateSurroundings(List<GameObject[]> placeholders)
    {
        List<GameObject> surroundings = ArrangeSurroundings(placeholders);
        //fill gaps
        SetParentAll(surroundings, surroundingsGathererPrefab);
    }

    //separate left and right walls to easier fill gaps later
    private List<GameObject> ArrangeSurroundings(List<GameObject[]> placeholders)
    {
        List<GameObject> surroundings = new List<GameObject>();
        Transform wall = wallPrefab.transform;
        //arrange start placeholder
        for(int i = 1; i < placeholders.Count - 1; i++)
        {
            Transform previousOuterPlaceholder = placeholders[i - 1][1].transform;
            Transform currentOuterPlaceholder = placeholders[i][1].transform;
            float previousYRotation = previousOuterPlaceholder.rotation.eulerAngles.y;
            float currentYRotation = currentOuterPlaceholder.root.eulerAngles.y;
            if (previousYRotation == currentYRotation)
            {
                GameObject leftWall;
                GameObject rightWall;
                InstantiateSideWalls(currentOuterPlaceholder, out leftWall, out rightWall);
                if (leftWall != null)
                {
                    surroundings.Add(leftWall);
                }
                if (rightWall != null)
                {
                    surroundings.Add(rightWall);
                }
            }
            else
            {
                float side = MathMethod.VectorSign(previousOuterPlaceholder.forward);
                GameObject sideWall = InstantiateSideWall(currentOuterPlaceholder, side);
                GameObject backWall = InstantiateBackWall(currentOuterPlaceholder);
                ScaleDownWall(previousOuterPlaceholder, backWall.transform);
                surroundings.Add(sideWall);
                surroundings.Add(backWall);
            }
        }
        //arrange end placeholder
        return surroundings;
    }
    //Assumption: there is always overlap
    private void ScaleDownWall(Transform previousPlaceholder, Transform wall)
    {
        float previousYRotation = previousPlaceholder.rotation.eulerAngles.y;
        float wallYRotation = wall.rotation.eulerAngles.y;
        float previousPosition = (previousYRotation % 180 == 0) ? previousPlaceholder.position.z : previousPlaceholder.position.x;
        float wallPosition = (wallYRotation % 180 == 0) ? wall.position.x : wall.position.z;
        float previousOffset = (previousPlaceholder.lossyScale.z / 2);
        float wallOffest = (wall.lossyScale.x / 2);
        float overlap;
        if ((previousYRotation == 0) || (previousYRotation == 90))
        {
            overlap = (previousPosition + previousOffset) - (wallPosition - wallOffest);
        }
        else
        {
            overlap = (wallPosition + wallOffest) - (previousPosition - previousOffset);
        }
        Vector3 direction = (previousPlaceholder.forward == wall.right) ? wall.right : -wall.right;
        wall.localScale -= new Vector3(overlap, 0, 0);
        wall.position += (direction * (overlap / 2));
    }
    private PlaceholderFlag GetFlag(Transform placeholder)
    {
        PlaceholderFlag flag = placeholder.gameObject.GetComponent<PlaceholderFlag>();
        return flag;
    }
    private GameObject InstantiateSideWall(Transform placeholder, float side)
    {
        Vector3 wallScale = new Vector3(1, placeholder.lossyScale.y, placeholder.lossyScale.z);
        Vector3 offsetDirection = (placeholder.rotation.eulerAngles.y % 180 == 0) ? Vector3.right : Vector3.forward;
        Vector3 wallOffset = offsetDirection * ((placeholder.lossyScale.x / 2) - (wallScale.x / 2));
        Vector3 wallPosition = placeholder.position + (side * wallOffset);
        GameObject wall = Instantiate(wallPrefab, wallPosition, placeholder.rotation);
        wall.transform.localScale = wallScale;
        return wall;
    }
    private GameObject InstantiateBackWall(Transform placeholder)
    {
        Vector3 wallScale = new Vector3(placeholder.lossyScale.x, placeholder.lossyScale.y, 1);
        Vector3 wallOffset = placeholder.forward * ((placeholder.lossyScale.z / 2) - (wallScale.z / 2));
        Vector3 wallPosition = placeholder.position - wallOffset;
        GameObject wall = Instantiate(wallPrefab, wallPosition, placeholder.rotation);
        wall.transform.localScale = wallScale;
        return wall;
    }
    private void InstantiateSideWalls(Transform currentPlaceholder, out GameObject leftWall, out GameObject rightWall)
    {
        PlaceholderFlag flag = GetFlag(currentPlaceholder);
        if (flag != null)
        {
            float side = MathMethod.VectorSign(flag.OccupiedSide);
            if (side == 1)
            {
                leftWall = InstantiateSideWall(currentPlaceholder, -side);
                rightWall = null;
            }
            else
            {
                leftWall = null;
                rightWall = InstantiateSideWall(currentPlaceholder, -side);
            }
        }
        else
        {
            leftWall = InstantiateSideWall(currentPlaceholder, -1);
            rightWall = InstantiateSideWall(currentPlaceholder, 1);                    
        }
    }
    private void SetParentAll(List<GameObject> gameObjects, GameObject parent)
    {
        foreach (GameObject gameObject in gameObjects)
        {
            gameObject.transform.SetParent(parent.transform);
        }
    }
}
