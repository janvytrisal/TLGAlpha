using UnityEngine;
using System.Collections;
using UnityUtilityLibrary;

/*
 * Rotate platform around X, Y, or Z axis.
 * UNFINISHED -> check how it works with rotation
 */
public class CircularPlatformMotion : PlatformMotion 
{
    private Vector3 _center;
    private float _angle;
    private Vector3 _rotationAxis;

    private Vector3 _offset;
    private float _traveledDistance;
    private Vector3 _previousRotatedPosition; //replace

    public DataType.Axis axis;
    public DataType.Sign axisOrientation;

    void Awake()
    {
        _center = GetDefaultCenter(StartingPosition);
        _angle = 360 * (speed / GetCircleCircumference());
        _rotationAxis = GetDefaultRotationAxis();
        _traveledDistance = 0;
        _offset = transform.position - _center;
        _previousRotatedPosition = transform.position;
        StartingPosition = transform.position;
    }
    void FixedUpdate()
    {
        Vector3 rotationVelocity = ComputeRotationVelocity();
        MoveAll(rotationVelocity);
        UpdateTraveledDistance(rotationVelocity);
    }

    public override Vector3 GetDestinationPosition()
    {
        Vector3 destinationOffset = 2 * (GetDefaultCenter(StartingPosition) - StartingPosition);
        return StartingPosition + (transform.rotation * destinationOffset);
    }
    public override void ShiftToDestinationPosition()
    {
        transform.position = GetDestinationPosition();
        _traveledDistance = 0;
        DestinationReached = true;
        _offset = GetDefaultCenter(StartingPosition) - StartingPosition; 
        _previousRotatedPosition = GetDefaultCenter(StartingPosition) + _offset; 
    }
    public override Vector3 GetMotionCenter()
    {
        Vector3 centerOffset = GetDefaultCenter(StartingPosition) - StartingPosition;
        Vector3 rotatedCenter = StartingPosition + (transform.rotation * centerOffset);
        return rotatedCenter;
    }
    public override Vector3 GetMotionExtents()
    {
        Vector3 extents = transform.lossyScale;
        float diameter = 2 * GetRadius();
        if (axis == DataType.Axis.X)
        {
            extents.y += diameter;
            extents.z += diameter;
        }
        else if (axis == DataType.Axis.Y)
        {
            extents.x += diameter;
            extents.z += diameter;
        }
        else
        {
            extents.x += diameter;
            extents.y += diameter;
        }
        Vector3 rotatedExtents = transform.rotation * extents;
        return rotatedExtents;
    }

    private Vector3 ComputeRotationVelocity()
    {
        Quaternion rotation = Quaternion.AngleAxis(_angle * Time.deltaTime, _rotationAxis);
        _offset = rotation * _offset;
        Vector3 newRotatedPosition = _center + _offset;
        Vector3 rotationVelocity = newRotatedPosition - _previousRotatedPosition; //rotatedPosition does not account for transform.rotation, while tr.pos is already world
        _previousRotatedPosition = newRotatedPosition;
        return rotationVelocity;
    }
    private void UpdateTraveledDistance(Vector3 velocity)
    {
        float stepDistance = velocity.magnitude;
        _traveledDistance += stepDistance;
        if(MathMethod.Approximately(_traveledDistance, travelDistance, 3))
        {
            _traveledDistance = 0;
            DestinationReached = !DestinationReached;
        }
    }
    private float GetCircleCircumference()
    {
        return (2 * travelDistance);
    }
    private float GetRadius()
    {
        return travelDistance / Mathf.PI; //because r = c / (2pi), where c = 2travelDistance
    }
    private Vector3 GetDefaultCenter(Vector3 startingPosition)
    {
        Vector3 center = startingPosition;
        float radius = GetRadius();
        if (axis == DataType.Axis.X)
        {
            center.z += radius;
        }
        else if (axis == DataType.Axis.Y)
        {
            center.z += radius;
        }
        else
        {
            center.y += radius;
        }
        return center;
    }
    private Vector3 GetDefaultRotationAxis()
    {
        Vector3 rotationAxis = Vector3.zero;
        if (axis == DataType.Axis.X)
        {
            rotationAxis = (int)axisOrientation * Vector3.right;
        }
        else if (axis == DataType.Axis.Y)
        {
            rotationAxis = (int)axisOrientation * Vector3.up;
        }
        else
        {
            rotationAxis = (int)axisOrientation * Vector3.forward;
        }
        return rotationAxis;
    }
}
