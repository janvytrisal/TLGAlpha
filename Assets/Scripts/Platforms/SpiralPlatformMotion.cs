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
 * Rotate and move platform along X, Y or Z axis. Back and forth, in spiral motion.
 * UNFINISHED -> check rotations
 */
public class SpiralPlatformMotion : PlatformMotion 
{
    private Vector3 _center;
    private float _angle;
    private Vector3 _direction;

    private Vector3 _offset;
    private float _loopCounter;
    private float _loopTraveledDistance;
    private Vector3 _previousRotatedPosition;

    //[Range(1, 5)]
    public float loops;
    //public float radius;
    public DataType.Axis motionAxis;
    public DataType.Sign axisOrientation;


    void Awake() //called when object is instantiated
    {
        StartingPosition = transform.position;
        _center = GetDefaultCenter(StartingPosition);
        _angle = 360 * (speed / GetCircleCircumference());
        _direction = GetDefaultDirection();
        _loopCounter = 0;
        _loopTraveledDistance = 0;
        DestinationReached = false;
        _offset = transform.position - _center;
        _previousRotatedPosition = transform.position;
    }
    void FixedUpdate()
    {
        Vector3 rotationVelocity = ComputeRotationVelocity();
        Vector3 straightVelocity = _direction * speed * Time.deltaTime;
        MoveAll(rotationVelocity + straightVelocity);
        UpdateDirection(straightVelocity);
    }
        
    public override Vector3 GetDestinationPosition()
    {
        Vector3 destinationOffset = GetDefaultDirection() * travelDistance;
        if (loops % 2 != 0)
        {
            destinationOffset += 2 * (GetDefaultCenter(StartingPosition) - StartingPosition);
        }
        return StartingPosition + (transform.rotation * destinationOffset);
    }
    public override void ShiftToDestinationPosition()
    {
        transform.position = GetDestinationPosition();
        _direction = (GetDefaultDirection() * -1);
        _angle = ((360 * (speed / GetCircleCircumference())) * -1);
        _loopCounter = 0;
        DestinationReached = true;
        if (loops % 2 != 0) //replace
        {
            _offset = GetDefaultCenter(StartingPosition) - StartingPosition; //.normalized * GetRadius();
            _previousRotatedPosition = GetDefaultCenter(StartingPosition) + _offset;
        }
    }
    public override Vector3 GetMotionCenter()
    {
        Vector3 start = StartingPosition;
        Vector3 end = GetDestinationPosition();
        Vector3 offset = end - start;
        Vector3 center = start + (offset / 2);
        return center;
    }
    public override Vector3 GetMotionExtents()
    {
        Vector3 start = StartingPosition;
        Vector3 end = GetDestinationPosition();
        Vector3 offset = end - start;
        offset.x = Mathf.Abs(offset.x);
        offset.y = Mathf.Abs(offset.y);
        offset.z = Mathf.Abs(offset.z);
        Vector3 extents = transform.lossyScale;
        float diameter = 2 * GetRadius();
        if (motionAxis == DataType.Axis.X)
        {
            extents.y += diameter;
            extents.z += diameter;
        }
        else if (motionAxis == DataType.Axis.Y)
        {
            extents.x += diameter;
            extents.z += diameter;
        }
        else
        {
            extents.x += diameter;
            extents.y += diameter;
        }
        Vector3 rotatedExtents = offset + (transform.rotation * extents);
        return rotatedExtents;
    } 

    private Vector3 ComputeRotationVelocity()
    {
        Quaternion rotation = Quaternion.AngleAxis(_angle * Time.deltaTime, _direction);
        _offset = rotation * _offset;
        Vector3 newRotatedPosition = _center + _offset;
        Vector3 rotationVelocity = newRotatedPosition - _previousRotatedPosition;
        _previousRotatedPosition = newRotatedPosition;
        return rotationVelocity;
    }
    private void UpdateDirection(Vector3 straightVelocity)
    {
        float stepDistance = straightVelocity.magnitude;
        _loopTraveledDistance += stepDistance;
        if(MathMethod.Approximately(_loopTraveledDistance, GetCircleCircumference() / 2, 3))
        {
            _loopCounter += 0.5f;
            _loopTraveledDistance = 0;
            if (_loopCounter >= loops)
            {
                _direction *= -1;
                _angle *= -1;
                _loopCounter = 0;
                DestinationReached = !DestinationReached;
            }
        }
    }
    private float GetCircleCircumference()
    {
        return travelDistance / loops;
    }
    private float GetRadius()
    {
        return travelDistance / (2 * loops * Mathf.PI); //because r = c / (2pi), where c = 2travelDistance
    }
    private Vector3 GetDefaultCenter(Vector3 startingPosition)
    {
        Vector3 center = startingPosition;
        float radius = GetRadius();
        if (motionAxis == DataType.Axis.X)
        {
            center.z += radius;
        }
        else if (motionAxis == DataType.Axis.Y)
        {
            center.z += radius;
        }
        else
        {
            center.y += radius;
        }
        return center;
    }
    private Vector3 GetDefaultDirection()
    {
        Vector3 direction = Vector3.zero;
        if (motionAxis == DataType.Axis.X)
        {
            direction = (int)axisOrientation * Vector3.right;
        }
        else if (motionAxis == DataType.Axis.Y)
        {
            direction = (int)axisOrientation * Vector3.up;
        }
        else
        {
            direction = (int)axisOrientation * Vector3.forward;
        }
        return direction;
    }
}
