using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityUtilityLibrary;

/*
 * Rotates main camera when accelerometers x value reach certain threshold.
 * This happen when player move one side of device up.
 * Rotation is always 90 degrees executed as coroutine.
 * TO DO: check if angle after rotation doesn't have too large imprecision
 */
public class AccelerometerPlayerCamera : MonoBehaviour 
{
    private float _rotationThreshold = 0.4f;
    private float _canRotateResetThreshold = 0.1f;
    private bool _performingRotation;
    private bool _canRotate;

    private enum RotationType { Left, Right, None };

    public Vector3 cameraOffset;
    public Vector3 cameraRotation;
    public float turnAngle;
    public float rotationSpeed;

    void Start()
    {
        _performingRotation = false;
        _canRotate = true;
    }
    void LateUpdate()
    {
        if (!_performingRotation)
        {
            RotationType neededRotation = GetNeededRotation();
            if (neededRotation == RotationType.Right)
            {
                StartCoroutine(DoRotation(-turnAngle, neededRotation)); 
            }
            else if (neededRotation == RotationType.Left)
            {
                StartCoroutine(DoRotation(turnAngle, neededRotation)); 
            }
        }
        Camera.main.transform.position = transform.position + cameraOffset;
        Camera.main.transform.rotation = Quaternion.Euler(cameraRotation);
    }

    private RotationType GetNeededRotation()
    {
        if ((Input.acceleration.x < -_rotationThreshold) && (_canRotate))
        {
            return RotationType.Right;
        }
        else if ((Input.acceleration.x > _rotationThreshold) && (_canRotate))
        {
            return RotationType.Left;
        }
        else if ((Input.acceleration.x >= -_canRotateResetThreshold) && (Input.acceleration.x <= _canRotateResetThreshold))
        {
            _canRotate = true; //set elsewhere
        }
        return RotationType.None;
    }
    private IEnumerator DoRotation(float angleToTurn, RotationType rotationType) //DoRotation -> Rotate
    {
        _performingRotation = true;
        float deltaAngle = rotationSpeed * Time.deltaTime;
        if (rotationType == RotationType.Right)
        {
            deltaAngle *= -1; 
        }
        float angleTotal = 0;
        while(!MathMethod.Approximately(angleTotal, angleToTurn, 3)) //replace
        {
            if (Mathf.Abs(angleTotal + deltaAngle) > Mathf.Abs(angleToTurn))
            {
                deltaAngle = angleToTurn - angleTotal;
            }
            angleTotal += deltaAngle;
            cameraRotation.y += deltaAngle;
            RotateCameraOffset(deltaAngle);
            yield return null;
        }
        _canRotate = false;
        _performingRotation = false;
    }
    private void RotateCameraOffset(float deltaAngle)
    {
        Quaternion rotation = Quaternion.AngleAxis(deltaAngle, Vector3.up);
        cameraOffset = rotation * cameraOffset;
    }
}
