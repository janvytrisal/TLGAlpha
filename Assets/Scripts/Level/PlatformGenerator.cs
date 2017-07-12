using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityUtilityLibrary;

[System.Serializable]
public class PrefabArray
{
    public string type;
    public GameObject[] prefabs;
}

/*
 * Generate level from platforms. Each platform have two placeholders: inner and outer.
 * Inner represents platforms motion extents. Outer represents world space 
 * reserved for the platform, it's enemies and surroundings.
 * 
 * ArrangePlatforms pseudocode:
 * lineLengths = divide platforms to lines of chosen length
 * foreach line in lineLengths
 *  chose direction
 *  try building a line of platforms in chosen direction
 *  if(cant build because any placeholder touch previously build placeholder)
 *      if(more directions are available)
 *          undo current platform line
 *      else
 *          undo current and previous line
 *      jump to "chose direction"
 * 
 * ArrangePlatforms is executed as coroutine because of computational (time) complexity.
 * OPTIMISE -> make IsOverlapping check less placeholders
 */
public class PlatformGenerator : MonoBehaviour 
{
    private List<GameObject> _platforms;
    private int _platformCount;
    private int _seed;
    private Vector3 _defaultPosition = new Vector3(0, 0, -9999);
    private List<GameObject[]> _placeholders;
    private int _minLineLength = 2;
    private int _maxLineLength = 2;
    private float _extensionWidth = 10;
    private float _extensionHeight = 10;
    private float _minGap = 0.01f; //so objects never touch

    public GameObject basicPlatformPrefab;
    public GameObject startPlatformPrefab;
    public GameObject endPlatformPrefab;
    //public PrefabArray[] staticPlatformPrefabs;
    public PrefabArray[] movingPlatformPrefabs; //jagged array workaround
    public GameObject platformsGathererPrefab;

    public IEnumerator GeneratePlatforms(int staticCount, int movingCount)
    {
        _platforms = new List<GameObject>();
        _platformCount = staticCount + movingCount;

        List<GameObject> platforms = SelectPlatforms(staticCount, movingCount);
        _platforms = InstantiatePlatforms(platforms);
        _placeholders = InstantiatePlaceholders(_platforms);
        yield return StartCoroutine(ArrangePlatforms(_platforms, _placeholders));
        FlagPlaceholders(_placeholders);

        GameManager.Platforms = _platforms;
        GameManager.Placeholders = _placeholders;
        //SetParentAll(_platforms, platformsGathererPrefab);
    }
    public void DestroyAllPlaceholders()
    {
        foreach (GameObject[] placeholders in _placeholders)
        {
            Destroy(placeholders[0]);
            Destroy(placeholders[1]);
        } 
    }
    public void DeactiveAllPlaceholders()
    {
        foreach (GameObject[] placeholders in _placeholders)
        {
            placeholders[0].SetActive(false);
            placeholders[1].SetActive(false);
        }
    }

    private List<GameObject> SelectPlatforms(int staticCount, int movingCount) 
    {
        List<GameObject> platforms = new List<GameObject>();
        platforms.Add(startPlatformPrefab);
        for(int i = 0; i < _platformCount; i++)
        {
            //platform sorting: static_0, static_1, ... , static_m, moving_0, moving_1, ... , moving_n
            int platformNumber = Random.Range(0, staticCount + movingCount);
            if (platformNumber < staticCount)
            {
                platforms.Add(basicPlatformPrefab);
                staticCount--;
            }
            else
            {
                GameObject movingPlatform = SelectPlatform(movingPlatformPrefabs);
                movingPlatform.GetComponent<PlatformMotion>().enabled = false; //turn movement off by default
                platforms.Add(movingPlatform);
                movingCount--;
            }
        }
        platforms.Add(endPlatformPrefab);
        return platforms;
    }
    private GameObject SelectPlatform(PrefabArray[] platforms)
    {
        int type = 0; //Random.Range(0, platforms.Length);
        int i = Random.Range(0, platforms[type].prefabs.Length);
        GameObject selectedPlatform = platforms[type].prefabs[i];
        return selectedPlatform;
    }
    private List<GameObject> InstantiatePlatforms(List<GameObject> platforms)
    {
        List<GameObject> instantiatedPlatforms = new List<GameObject>();
        foreach (GameObject platform in platforms)
        {
            GameObject instantiatedPlatform = GameObject.Instantiate(platform, _defaultPosition, Quaternion.identity);
            instantiatedPlatform.SetActive(true);
            instantiatedPlatforms.Add(instantiatedPlatform); 
        }
        return instantiatedPlatforms;
    }
    private List<GameObject[]> InstantiatePlaceholders(List<GameObject> instantiatedPlatforms)
    {
        List<GameObject[]> placeholders = new List<GameObject[]>();
        foreach (GameObject platform in instantiatedPlatforms)
        {
            GameObject innerPlaceholder = GameObject.CreatePrimitive(PrimitiveType.Cube);
            Color randomColor = Random.ColorHSV();
            innerPlaceholder.GetComponent<MeshRenderer>().material.color = randomColor; //DEBUGING
            SetPlaceholderTransform(platform, innerPlaceholder);
            innerPlaceholder.name = "InnerPlaceholder";

            GameObject outerPlaceholder = GameObject.CreatePrimitive(PrimitiveType.Cube);
            outerPlaceholder.GetComponent<MeshRenderer>().material.color = randomColor;
            SetPlaceholderTransform(platform, outerPlaceholder);
            outerPlaceholder.name = "OuterPlaceholder";
            outerPlaceholder.layer = LayerMask.NameToLayer("OuterPlaceholder");
            outerPlaceholder.SetActive(false);

            innerPlaceholder.transform.SetParent(platform.transform);
            outerPlaceholder.transform.SetParent(platform.transform);

            placeholders.Add(new GameObject[]{innerPlaceholder, outerPlaceholder});
        }
        return placeholders;
    }
    private IEnumerator ArrangePlatforms(List<GameObject> platforms, List<GameObject[]> placeholders)
    {
        List<int> lineLengths = GenerateLineLengths(platforms.Count);
        List<float[]> directions = GenerateDefaultDirections(lineLengths.Count);
        ArrangeInitialLine(platforms, lineLengths[0], directions[0][0]);

        for (int i = 1; i < lineLengths.Count; i++)
        {
            int arrangedCount = lineLengths.GetRange(0, i).Sum();
            directions[i] = RemoveDirection(directions[i], GetBackwardDirection(platforms[arrangedCount - 1]));
            bool lineArranged = false;

            while ((directions[i].Length > 0) && (!lineArranged))
            {
                lineArranged = true;
                float direction = ChoseDirection(directions, i);

                for (int j = arrangedCount; j < (arrangedCount + lineLengths[i]); j++)
                {
                    SetPlatformParameters(platforms[j - 1], platforms[j], direction);
                    bool allowed = !IsOverlapping(placeholders[j][0], placeholders, j);
                    if (allowed)
                    { 
                        SetOuterPlaceholder(platforms[j - 1], platforms[j]);
                        allowed = !IsOverlapping(placeholders[j][1], placeholders, j, arrangedCount - 1);
                    }
                    if (!allowed)
                    { 
                        ResetPlatformLine(platforms, placeholders, arrangedCount, j - arrangedCount + 1);
                        lineArranged = false;
                        break;
                    }
                }
            }
            if (!lineArranged)
            {
                directions = RebuildDirections(directions, i);
                ResetPlatformLine(platforms, placeholders, arrangedCount - lineLengths[i - 1], lineLengths[i - 1]);
                i -= 2;
            }
            yield return null; 
        }
    }
    private void SetActiveAll(List<GameObject> gameObjects, bool value)
    {
        foreach (GameObject gameObject in gameObjects)
        {
            gameObject.SetActive(value);
        }
    }
    private void SetPositionAll(List<GameObject> gameObjects, Vector3 position)
    {
        foreach (GameObject gameObject in gameObjects)
        {
            gameObject.transform.position = position;
        }
    }
    private void DestroyAll(List<GameObject> gameObjects)
    {
        foreach (GameObject gameObject in gameObjects)
        {
            Destroy(gameObject);
        } 
    }
    private float GetBackwardDirection(GameObject platform)
    {
        float forwardDirection = platform.transform.rotation.eulerAngles.y;
        return (forwardDirection + 180) % 360;
    }
    private void ArrangeInitialLine(List<GameObject> platforms, int lineLength, float direction)
    {
        platforms[0].transform.rotation = Quaternion.Euler(0, direction, 0);;
        platforms[0].transform.position = Vector3.zero;
        SetOuterPlaceholder(platforms[0], platforms[0]);

        for (int i = 1; i < lineLength; i++) 
        {
            SetPlatformParameters(platforms[i - 1], platforms[i], direction);
            SetOuterPlaceholder(platforms[i - 1], platforms[i]);
        }
    }
    private List<int> GenerateLineLengths(int gameObjectsCount)
    {
        List<int> lineLengths = new List<int>();
        int lineLengthsTotal = 0;
        while (lineLengthsTotal < gameObjectsCount)
        {
            int lineLength = Random.Range(_minLineLength, _maxLineLength + 1);
            if (lineLength + lineLengthsTotal > gameObjectsCount)
            {
                lineLength = gameObjectsCount - lineLengthsTotal;
            }
            lineLengths.Add(lineLength);
            lineLengthsTotal += lineLength;
        }
        return lineLengths;
    }
    private List<GameObject[]> ToLines(List<GameObject> gameObjects, List<int> lineLengths)
    {
        List<GameObject[]> lines = new List<GameObject[]>();
        int currentGameObject = 0;
        foreach (int length in lineLengths)
        {
            GameObject[] line = new GameObject[length];
            for (int i = 0; i < length; i++)
            {
                line[i] = gameObjects[currentGameObject];
                currentGameObject++;
            }
            lines.Add(line);
        }
        return lines;
    }
    private List<float[]> GenerateDefaultDirections(int lineLengthCount)
    {
        List<float[]> directions = new List<float[]>();
        float[] defaultDirections = new float[]{ 0, 90, 180, 270 };

        directions.Add(new float[]{ 0 });
        for (int i = 1; i < lineLengthCount; i++)
        {
            directions.Add(defaultDirections);
        }
        return directions;
    }
    private List<float[]> RebuildDirections(List<float[]> directions, int startIndex)
    {
        List<float[]> rebuildedDirections = new List<float[]>();
        for(int i = 0; i < startIndex; i++)
        {
            rebuildedDirections.Add(directions[i]);
        }

        float[] defaultDirections = new float[]{ 0, 90, 180, 270 };
        for(int i = startIndex; i < directions.Count; i++)
        {
            rebuildedDirections.Add(defaultDirections);
        }
        return rebuildedDirections;
    }
    private Vector3 CreateNewPosition(GameObject previousPlatform, Transform currentPlatform)
    {
        Transform previousPlatformTransform = previousPlatform.transform;
        Vector3 newPositionOffset = CreateNewPositionOffset(previousPlatformTransform, currentPlatform); 

        Vector3 previousDestinationPosition;
        PlatformMotion motion = previousPlatform.GetComponent<PlatformMotion>();
        if (motion != null)
        {
            previousDestinationPosition = motion.GetDestinationPosition();
        }
        else
        {
            previousDestinationPosition = previousPlatformTransform.position;
        }

        Vector3 currentPosition = previousDestinationPosition + newPositionOffset;
        return currentPosition;
    }
    private Vector3 CreateNewPositionOffset(Transform previousPlatform, Transform currentPlatform)
    {
        float previousZScale = previousPlatform.localScale.z;
        float positionOffsetScalar = previousZScale / 2;
        float previousYRotation = previousPlatform.rotation.eulerAngles.y;

        Vector3 currentLocalScale = currentPlatform.localScale;
        float currentYRotation = currentPlatform.rotation.eulerAngles.y;

        if (previousYRotation % 180 == 0)
        {
            if (currentYRotation % 180 == 0)
            {
                positionOffsetScalar += currentLocalScale.z / 2;
            }
            else
            {
                positionOffsetScalar += currentLocalScale.x / 2;
            }
        }
        else
        {
            if (currentYRotation % 180 == 0)
            {
                positionOffsetScalar += currentLocalScale.x / 2;
            }
            else
            {
                positionOffsetScalar += currentLocalScale.z / 2;
            }
        }
        Vector3 positionOffset = previousPlatform.forward * (positionOffsetScalar + GenerateGap());
        return positionOffset;
    }
    private float GenerateGap()
    {
        int maxGap = 3; //(((int)GameManager.Player.GetComponent<PlayerMotions>().jumpSpeed) / 2);
        float baseGap = (float)Random.Range(0, maxGap);
        baseGap = (baseGap == 1) ? 0 : baseGap;
        return baseGap + _minGap;
    }
    //setting inner placeholder
    private void SetPlaceholderTransform(GameObject platform, GameObject placeholder)
    {
        Transform placeholderTransform = placeholder.transform;
        PlatformMotion motion = platform.GetComponent<PlatformMotion>();
        if(motion != null)
        {
            placeholderTransform.position = motion.GetMotionCenter(); 
            placeholderTransform.localScale = motion.GetMotionExtents();
        }
        else
        {
            placeholderTransform.position = platform.transform.position;
            placeholderTransform.localScale = platform.transform.localScale;
        }
    }
    //placeholder to hold action space for whole platform: innerPlaceholder, enemies, walls, gap
    private void SetOuterPlaceholder(GameObject previousPlatform, GameObject currentPlatform)
    {
        Vector3 extents = GetPlatformExtents(currentPlatform);
        Vector3 extension = new Vector3(_extensionWidth, _extensionHeight, 0);
        extents += extension;
        extents = MathMethod.LossyToLocal(extents, currentPlatform.transform.lossyScale);
        Transform outerPlaceholderTransform = currentPlatform.transform.Find("OuterPlaceholder");
        Transform innerPlaceholderTransform = currentPlatform.transform.Find("InnerPlaceholder");
        outerPlaceholderTransform.gameObject.SetActive(true);
        outerPlaceholderTransform.localScale = extents;
        outerPlaceholderTransform.localPosition = innerPlaceholderTransform.localPosition;

        float gap;
        if (previousPlatform.transform.rotation == currentPlatform.transform.rotation)
        {
            gap = GetBetweenGap(previousPlatform.transform, currentPlatform.transform);
        }
        else
        {
            gap = GetCornerGap(previousPlatform.transform, currentPlatform.transform);
        }
        float localGap = MathMethod.LossyToLocal(gap, currentPlatform.transform.lossyScale.z);
        outerPlaceholderTransform.localScale += new Vector3(0, 0, localGap);
        outerPlaceholderTransform.localPosition += new Vector3(0, 0, -localGap / 2);
    }
    private float GetBetweenGap(Transform previousPlatform, Transform currentPlatform)
    {
        float gap;
        Transform previousPlaceholder = previousPlatform.Find("InnerPlaceholder");
        Transform currentPlaceholder = currentPlatform.Find("InnerPlaceholder");
        float yRotation = currentPlatform.rotation.eulerAngles.y; //same for previous
        float currentPosition = (yRotation % 180 == 0)? currentPlaceholder.position.z : currentPlaceholder.position.x; //global to local coords conversion
        float previousPosition = (yRotation % 180 == 0)? previousPlaceholder.position.z : previousPlaceholder.position.x;

        if (currentPosition > previousPosition)
        {
            gap = (currentPosition - (currentPlaceholder.lossyScale.z / 2)) - (previousPosition + (previousPlaceholder.lossyScale.z / 2));
        }
        else
        {
            gap = (previousPosition - (previousPlaceholder.lossyScale.z / 2)) - (currentPosition + (currentPlaceholder.lossyScale.z / 2));
        }

        gap = Mathf.Round(gap);
        gap = (gap < 0)? 0 : gap; //because SetOuterPlaceholder(start, start), refactor later
        return gap;
    }
    private float GetCornerGap(Transform previousPlatform, Transform currentPlatform)
    {
        float gap;
        Transform previousPlaceholder = previousPlatform.Find("OuterPlaceholder");
        Transform currentPlaceholder = currentPlatform.Find("OuterPlaceholder");
        float previousYRotation = previousPlatform.rotation.eulerAngles.y;
        float currentYRotation = currentPlatform.rotation.eulerAngles.y;

        float previousPosition = (previousYRotation % 180 == 0) ? previousPlaceholder.position.x : previousPlaceholder.position.z;
        float currentPosition = (currentYRotation % 180 == 0) ? currentPlaceholder.position.z : currentPlaceholder.position.x;
        float previousOffset = previousPlaceholder.lossyScale.x / 2;
        float currentOffset = currentPlaceholder.lossyScale.z / 2;

        if ((currentYRotation == 0) || (currentYRotation == 90))
        {
            gap = (currentPosition - currentOffset) - (previousPosition - previousOffset);
        }
        else //180, 270
        {
            gap = (previousPosition + previousOffset) - (currentPosition + currentOffset);
        }
        return gap;
    }
    private Vector3 GetPlatformExtents(GameObject platform)
    {
        Vector3 extents;
        PlatformMotion motion = platform.GetComponent<PlatformMotion>();
        if (motion != null)
        {
            extents = motion.GetMotionExtents(); 
        }
        else
        {
            extents = platform.transform.lossyScale;
        }
        return extents;
    }
    private void SetPlatformParameters(GameObject previousPlatform, GameObject currentPlatform, float direction)
    {
        currentPlatform.transform.rotation = Quaternion.Euler(0, direction, 0);
        currentPlatform.transform.position = CreateNewPosition(previousPlatform, currentPlatform.transform);
        SetStartingPosition(currentPlatform);
    }
    private void SetStartingPosition(GameObject platform)
    {
        PlatformMotion motion = platform.GetComponent<PlatformMotion>();
        if (motion != null)
        {
            motion.StartingPosition = platform.transform.position;
        }
    }
    private void ResetPlatformLine(List<GameObject> platforms, List<GameObject[]> placeholders, int fromIndex, int count)
    {
        SetPositionAll(platforms.GetRange(fromIndex, count), _defaultPosition);
        SetActiveAll(placeholders.GetRange(fromIndex, count).Select(x => x[1]).ToList(), false);
    }
    private float ChoseDirection(List<float[]> directions, int directionIndex)
    {
        int k = Random.Range(0, directions[directionIndex].Length);
        float direction = directions[directionIndex][k];
        directions[directionIndex] = RemoveDirection(directions[directionIndex], direction);
        return direction;
    }
    private float[] RemoveDirection(float[] directions, float directionToRemove)
    {
        float[] remainingDirections = directions.Except(new float[]{ directionToRemove }).ToArray();
        return remainingDirections;
    }
    private void FlagPlaceholders(List<GameObject[]> placeholders)
    {
        for (int i = 1; i < placeholders.Count; i++)
        {
            Transform previous = placeholders[i - 1][1].transform;
            Transform current = placeholders[i][1].transform;
            if (previous.forward != current.forward)
            {
                int j = i;
                Collider[] overlappingColliders;
                bool overlap;
                do
                {
                    current = placeholders[j][1].transform;
                    overlappingColliders = Physics.OverlapBox(current.position, current.lossyScale / 2, current.rotation, ~current.gameObject.layer);
                    overlap = ContainsPlaceholder(previous.gameObject, overlappingColliders);
                    if (overlap)
                    {
                        AssignFlag(previous, current);
                    } 
                    j++;
                }
                while(overlap && (j < placeholders.Count));
                i = (!overlap) ? (j - 2) : j;
            }
        }
    }
    private bool ContainsPlaceholder(GameObject placeholder, Collider[] colliders)
    {
        foreach (Collider collider in colliders)
        {
            if (collider.gameObject == placeholder)
            {
                return true;
            }
        }
        return false;
    }
    private void AssignFlag(Transform previousPlaceholder, Transform currentPlaceholder)
    {
        PlaceholderFlag flag = currentPlaceholder.gameObject.AddComponent<PlaceholderFlag>();
        if (previousPlaceholder.forward == currentPlaceholder.right)
        {
            flag.OccupiedSide = -currentPlaceholder.right;
        }
        else
        {
            flag.OccupiedSide = currentPlaceholder.right;
        }
    }
    private List<GameObject> GetPotentialPlaceholders(List<GameObject[]> placeholders, int count, int allowedIndex = -1)
    {
        List<GameObject> outerPlaceholders = placeholders.GetRange(0, count).Select(x => x[1]).ToList();
        if (allowedIndex != -1)
        {
            outerPlaceholders.RemoveAt(allowedIndex);
        }
        return outerPlaceholders;
    }
    //optimise later
    private bool IsOverlapping(GameObject currentPlaceholder, List<GameObject[]> placeholders, int count, int allowedIndex = -1)
    {
        Bounds currentBounds = currentPlaceholder.GetComponent<MeshRenderer>().bounds;
        List<GameObject> potentialPlaceholders = GetPotentialPlaceholders(placeholders, count, allowedIndex);
        foreach (GameObject placeholder in potentialPlaceholders)
        {
            Bounds bounds = placeholder.GetComponent<MeshRenderer>().bounds;
            if (currentBounds.Intersects(bounds))
            {
                return true;
            }
        }
        return false;
    }
    private void SetParentAll(List<GameObject> gameObjects, GameObject parent)
    {
        foreach (GameObject gameObject in gameObjects)
        {
            gameObject.transform.SetParent(parent.transform);
        }
    }

    /* //probably more performance effective than IsOverlaping, but Physics.OverlapBox cause fatal error on android -> wait until fixed
    private bool AllowedOverlapping(Transform placeholder, params Transform[] allowedPlatforms)
    {
        bool allowed;
        //Debug.Log("Before Physics.OverlapBox");
        Collider[] overlappingColliders = Physics.OverlapBox(placeholder.position, placeholder.lossyScale / 2, placeholder.rotation);
        //Debug.Log("After Physics.OverlapBox");
        allowed = CollidersParentMatch(overlappingColliders, allowedPlatforms);
        return allowed;
    }
    //Check if colliders are part of any parent
    private bool CollidersParentMatch(Collider[] colliders, Transform[] parents)
    {
        foreach (Collider collider in colliders)
        {
            bool match = ParentMatch(collider.transform, parents);
            if (!match)
            {
                return false;
            }
        }
        return true;
    }
    private bool ParentMatch(Transform child, Transform[] parents)
    {
        if (child.parent != null)
        {
            return ParentMatch(child.parent, parents);
        }
        else
        {
            foreach (Transform parent in parents)
            {
                if (child == parent)
                {
                    return true;
                }
            }
            return false;
        }
    }*/
}
