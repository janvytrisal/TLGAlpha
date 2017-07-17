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
 * Proccessing touches to move camera position, rotation.
 * UNUSED
 */
public class PlayerCamera : MonoBehaviour 
{
    private RectTransform _topPanelHUD;
    private RectTransform _jumpPanel;
    private Vector2 _circlePoint; 
    private Vector2 _startPosition;

    public Vector3 cameraOffset; 
    public Vector3 cameraRotation;

    void Start()
    {
        _topPanelHUD = (RectTransform)GameObject.Find("HUDCanvas").transform.Find("TopPanel");
        _jumpPanel = (RectTransform)GameObject.Find("HUDCanvas").transform.Find("MainPanel").Find("RightHandPanel").Find("JumpPanel");
        _circlePoint = Vector2.up;
    }
    void LateUpdate()
    {
        ProcessTouches();

        UpdateCameraOffsetPosition();
        UpdateCameraRotation();
        Camera.main.transform.position = transform.position + cameraOffset;
        Camera.main.transform.rotation = Quaternion.Euler(cameraRotation);
        transform.forward = new Vector3(_circlePoint.x, 0, _circlePoint.y);
    }
        
    private void ProcessTouches() //update _circlePoint -> make it better
    {
        foreach (Touch touch in Input.touches)
        {
            if (touch.position.x > ((float)Screen.width / 2))
            {
                if (TouchWentToHUD(touch.position))
                {
                    continue;
                }
                if ((touch.phase == TouchPhase.Canceled) ||
                    (touch.phase == TouchPhase.Ended))
                {
                    continue;
                }
                if (touch.phase == TouchPhase.Began)
                {
                    _startPosition = touch.position;
                }
                float moveDistance = (touch.position - _startPosition).magnitude;
                float angle;
                Vector2 direction = touch.position - _startPosition;
                if (touch.phase == TouchPhase.Moved)
                {
                    _startPosition = touch.position;
                }
                if ((direction.x < 0) && (direction.y < 0))
                {
                    angle = -moveDistance;
                }
                else if ((direction.x > 0) && (direction.y > 0))
                {
                    angle = moveDistance;
                }
                else
                {
                    continue;
                }
                Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.up);
                Vector3 circlePoint = new Vector3(_circlePoint.x, 0, _circlePoint.y);
                circlePoint = rotation * circlePoint;
                _circlePoint.x = circlePoint.x;
                _circlePoint.y = circlePoint.z;
            }
        }
    }
    private bool TouchWentToHUD(Vector2 touchPosition)
    {
        Vector3 topHUDPosition = _topPanelHUD.position;
        Vector2 localTouch = new Vector2(touchPosition.x - topHUDPosition.x, touchPosition.y - topHUDPosition.y); //move touchPosition to panel's local space (assumption: panel's pivot is at 0,0)
        bool topHUD = _topPanelHUD.rect.Contains(localTouch);
        if (topHUD)
        {
            return true;
        }

        Vector3 jumpPanelPosition = _jumpPanel.position;
        localTouch = new Vector2(touchPosition.x - jumpPanelPosition.x, touchPosition.y - jumpPanelPosition.y);
        bool jumpPanel =_jumpPanel.rect.Contains(localTouch);
        if (jumpPanel)
        {
            return true;
        }
        return false;
    }
    private void UpdateCameraOffsetPosition()
    {
        float distance = new Vector2(cameraOffset.x, cameraOffset.z).magnitude; //r
        Vector2 newOffsetPosition = distance * -_circlePoint;
        cameraOffset = new Vector3(newOffsetPosition.x, cameraOffset.y, newOffsetPosition.y);
    }
    private void UpdateCameraRotation()
    {
        float rotationAngle = Mathf.Atan2(_circlePoint.x, _circlePoint.y) * Mathf.Rad2Deg;
        cameraRotation.y = rotationAngle;
    }
}
