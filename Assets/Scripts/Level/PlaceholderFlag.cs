using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * OccupiedSide holds transform.Right or -transform.Right vector representing which
 * side of placeholder can't be used when building surroundings or adding enemies.
 */
public class PlaceholderFlag : MonoBehaviour 
{
    private Vector3 _occupiedSide;

    public Vector3 OccupiedSide
    {
        get
        {
            return _occupiedSide;
        }
        set
        {
            _occupiedSide = value;
        }
    }


}
