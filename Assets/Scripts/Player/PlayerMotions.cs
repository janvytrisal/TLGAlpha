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
 * Deals with player motions. Handle touch inputs.
 * Left side of screen is for motion, on the right side is rectangle for jump action.
 * Rotation and movement are executed in FixedUpdate. 
 */
public class PlayerMotions : MonoBehaviour 
{
    private CharacterController _characterController;
    private Vector2 _startPosition;
    private Vector2 _direction;
    private Vector3 _velocity;
    private bool _jumped;
    private CollisionFlags _collisionFlags; //to make collision flags script independent (other scripts can call Move method freely)
    private RectTransform _topPanelHUD;
    private RectTransform _jumpPanel;

    public float movementSpeed;
    public float jumpSpeed;
    public Transform bodyTransform;

    public Vector2 Direction
    {
        get
        {
            return _direction;
        }
    }
    public Vector3 Velocity
    {
        get
        {
            return _velocity;
        }
        set
        {
            _velocity = value;
        }
    }

    void Start()
    {
        _characterController = GetComponent<CharacterController>();
        _startPosition = Vector2.zero;
        _direction = Vector2.zero;
        _velocity = Vector3.zero;
        _jumped = false;
        _topPanelHUD = (RectTransform)GameObject.Find("HUDCanvas").transform.Find("TopPanel");
        _jumpPanel = (RectTransform)GameObject.Find("HUDCanvas").transform.Find("MainPanel").Find("RightHandPanel").Find("JumpPanel");
	}
    void Update()
    {
        if (IsGrounded())
        {
            _velocity.y = 0;
            _direction = Vector2.zero;
        }
        #if UNITY_EDITOR
            ProcessKeyboardInput();
        #elif UNITY_ANDROID 
            ProcessTouches();
        #endif
    }
    //Do physics
    void FixedUpdate()
    {   
        if (_jumped)
        {
            _velocity.y = jumpSpeed;
            _jumped = false;
        }
        _velocity.y += 2 * Physics.gravity.y * Time.deltaTime;

        Vector2 correctedDirection = CorrectDirection();
        _velocity.x = correctedDirection.x * movementSpeed;
        _velocity.z = correctedDirection.y * movementSpeed;

        float eX = (2 * Mathf.Rad2Deg * _velocity.z * Time.deltaTime) / transform.localScale.x; //rotate to match velocity (ball rotation per second)
        float eZ = (-2 * Mathf.Rad2Deg * _velocity.x * Time.deltaTime) / transform.localScale.x;
        Vector3 eulerAngles = new Vector3(eX, 0, eZ);

        bodyTransform.Rotate(eulerAngles, Space.World);
        _collisionFlags = _characterController.Move(_velocity * Time.deltaTime); //velocity per second
    }

    public bool IsGrounded()
    {
        return ((_collisionFlags & CollisionFlags.Below) != 0);
    }
    public void Jump()
    {
        if (IsGrounded())
        {
            _jumped = true;
        }
    }

    private void ProcessKeyboardInput()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        _direction = (new Vector2(horizontal, vertical)).normalized;
        if (IsGrounded() && (Input.GetButtonDown("Jump")))
        {
            _jumped = true;
        }
    }
    private void ProcessTouches()
    {
        foreach (Touch touch in Input.touches)
        {
            if (TouchWentToHUD(touch.position))
            {
                continue;
            }

            if (touch.position.x <= ((float)Screen.width / 2))
            {
                if (IsGrounded())
                {
                    if (touch.phase == TouchPhase.Began)
                    {
                        _startPosition = touch.position;
                    }
                    else if ((touch.phase == TouchPhase.Ended) ||
                             (touch.phase == TouchPhase.Canceled))
                    {
                        _startPosition = Vector2.zero;
                        continue; //direction remains zero
                    }
                }
                else
                {
                    if ((touch.phase == TouchPhase.Began) && (_startPosition.magnitude == 0))
                    {
                        _startPosition = touch.position;
                    }
                }
                if (_startPosition != Vector2.zero) //to stop player from moving after respawn
                {
                    _direction = (touch.position - _startPosition).normalized; //joystick like motion
                }
            }
            else if (TouchWentToJumpPanel(touch.position))
            {
                if (IsGrounded() && (touch.phase == TouchPhase.Began))
                {
                    _jumped = true;
                }
            }
        }
    }

    private bool TouchWentToHUD(Vector2 touchPosition)
    {
        Vector3 hudPosition = _topPanelHUD.position;
        Vector2 localTouch = new Vector2(touchPosition.x - hudPosition.x, touchPosition.y - hudPosition.y); //move touchPosition to panel's local space (assumption: panel's world position is at lower left corner)
        return _topPanelHUD.rect.Contains(localTouch);
    }
    private bool TouchWentToJumpPanel(Vector2 touchPosition)
    {
        Vector3 jumpPanelPosition = _jumpPanel.position;
        Vector2 localTouch = new Vector2(touchPosition.x - jumpPanelPosition.x, touchPosition.y - jumpPanelPosition.y);
        return _jumpPanel.rect.Contains(localTouch);
    }
    private Vector2 CorrectDirection()
    {
        float yCameraAngle = GetComponent<AccelerometerPlayerCamera>().cameraRotation.y;
        Quaternion rotation = Quaternion.AngleAxis(yCameraAngle, Vector3.up);
        Vector3 direction = new Vector3(_direction.x, 0, _direction.y);
        direction = rotation * direction;
        return new Vector2(direction.x, direction.z);
    }
}