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

/*
 * Rotates main camera when accelerometers x value reach certain threshold.
 * This happen when player move one side of device up.
 * Rotation is always 90 degrees executed as coroutine.
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
            CanRotateCheck();
            RotationType neededRotation = GetNeededRotation();
            if (neededRotation == RotationType.Right)
            {
                StartCoroutine(DoRotation(-turnAngle)); 
            }
            else if (neededRotation == RotationType.Left)
            {
                StartCoroutine(DoRotation(turnAngle)); 
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
        return RotationType.None;
    }
    private void CanRotateCheck()
    {
        if ((Input.acceleration.x >= -_canRotateResetThreshold) && (Input.acceleration.x <= _canRotateResetThreshold))
        {
            _canRotate = true;
        }
    }
    private IEnumerator DoRotation(float angleToTurn) 
    {
        _performingRotation = true;
        float speed = (angleToTurn > 0) ? rotationSpeed : (rotationSpeed * -1);
        float absoluteAngleToTurn = Mathf.Abs(angleToTurn);
        float angleTotal = 0;
        do
        {
            float deltaAngle = speed * Time.deltaTime;
            if (Mathf.Abs(angleTotal + deltaAngle) > absoluteAngleToTurn)
            {
                deltaAngle = angleToTurn - angleTotal;
            }
            angleTotal += deltaAngle;
            cameraRotation.y += deltaAngle;
            RotateCameraOffset(deltaAngle);
            yield return null;
        }
        while(Mathf.Abs(angleTotal) < absoluteAngleToTurn);
        _canRotate = false;
        _performingRotation = false;
    }
    private void RotateCameraOffset(float deltaAngle)
    {
        Quaternion rotation = Quaternion.AngleAxis(deltaAngle, Vector3.up);
        cameraOffset = rotation * cameraOffset;
    }
}
