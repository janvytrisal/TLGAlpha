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
using UnityUtilityLibrary;

/*
 * Move platform along X and/or Y and/or Z axis. Back and forth.
 */
public class StraightPlatformMotion : PlatformMotion 
{
    private Vector3 _direction;
    private float _traveledDistance;

    [Range(-1, 1)]
    public int xAxis;
    [Range(-1, 1)]
    public int yAxis;
    [Range(0, 1)]
    public int zAxis;

    void Awake()
    {
        _direction = GetDefaultDirection();
        _traveledDistance = 0;
        DestinationReached = false;
        StartingPosition = transform.position;
    }
    void FixedUpdate()
    {
        Vector3 velocity = _direction * speed * Time.deltaTime;
        MoveAll(velocity);
        UpdateDirection(velocity);
    }

    public override Vector3 GetDestinationPosition()
    {
        Vector3 defaultDirection = GetDefaultDirection();
        Vector3 destinationOffset = defaultDirection * travelDistance;
        return StartingPosition + (transform.rotation * destinationOffset);
    }
    public override void ShiftToDestinationPosition()
    {
        transform.position = GetDestinationPosition();
        _direction = (GetDefaultDirection() * -1);
        _traveledDistance = 0;
        DestinationReached = true;
    }
    public override Vector3 GetMotionCenter()
    {
        Vector3 start = StartingPosition;
        Vector3 destination = GetDestinationPosition();
        Vector3 offset = destination - start;
        Vector3 center = start + (offset / 2);
        return center;
    }
    public override Vector3 GetMotionExtents()
    {
        Vector3 extents;
        Vector3 destinationOffset = GetDefaultDirection() * travelDistance;
        extents = transform.lossyScale + destinationOffset;
        return extents;
    }

    private void UpdateDirection(Vector3 velocity)
    {                                                                              
        float stepDistance = velocity.magnitude;
        _traveledDistance += stepDistance;
        if(MathMethod.Approximately(_traveledDistance, travelDistance, 3))
        {
            _direction *= -1;
            _traveledDistance = 0;
            DestinationReached = !DestinationReached;
        }
    }
    private Vector3 GetDefaultDirection()
    {
        return new Vector3(xAxis, yAxis, zAxis).normalized;
    }
}
