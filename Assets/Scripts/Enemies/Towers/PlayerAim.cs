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
 * Aim at predicted player position when he/she is in front of tower and nothing block view.
 * Sets ClearView property.
 */
public class PlayerAim : MonoBehaviour 
{
    private Vector3 _currentPlayerPosition;
    private Vector3 _previousPlayerPosition;
    private bool _clearView;
    private float _aimDistanceSquared; //performance optimisation
    private Vector3 _forwardDirection;
    private float _projectileSpeed;

    public float aimDistance;
    public GameObject projectilePrefab;

    public bool ClearView
    {
        get
        {
            return _clearView;
        }
    }

    void Start()
    {
        _aimDistanceSquared = aimDistance * aimDistance;
        _forwardDirection = -transform.forward;
        _forwardDirection.x = Mathf.Round(_forwardDirection.x);
        _forwardDirection.z = Mathf.Round(_forwardDirection.z);
        _projectileSpeed = projectilePrefab.GetComponent<ProjectileMotion>().speed;
    }
    void FixedUpdate()
    {
        UpdatePlayerPositions();
        LookAtPlayer();
    }

    private void UpdatePlayerPositions()
    {
        GameObject player = GameManager.Player;
        if (player != null)
        {
            _previousPlayerPosition = _currentPlayerPosition;
            _currentPlayerPosition = player.transform.position;
        }
    }
    private void LookAtPlayer()
    {
        Vector3 playerDirection = _currentPlayerPosition - transform.position;
        if (PlayerInFront(playerDirection) && (playerDirection.sqrMagnitude < _aimDistanceSquared))
        {
            if (!_clearView) //to lessen Raycasts
            {
                _clearView = ClearViewCheck(playerDirection);
            }
            if (_clearView)
            {
                Vector3 predictedPosition = PredictPlayerPosition(playerDirection);
                transform.LookAt(predictedPosition);
            }
        }
        else
        {
            _clearView = false;
        }
    }
    private bool ClearViewCheck(Vector3 playerDirection)
    {
        RaycastHit hitInfo;
        int layerMask = ~LayerMask.GetMask(new string[1]{"Enemy"}); 
        bool hit = Physics.Raycast(transform.position, playerDirection.normalized, out hitInfo, aimDistance, layerMask);
        if (hit)
        {
            if (hitInfo.collider.gameObject.tag == "Player")
            {
                return true;
            }
        }
        return false;
    }
    //simplify later
    private bool PlayerInFront(Vector3 playerDirection)
    {
        if (_forwardDirection.x != 0)
        {
            if (_forwardDirection.x == 1)
            {
                if (playerDirection.x > 0)
                {
                    return true;
                }
            }
            else
            {
                if (playerDirection.x < 0)
                {
                    return true;
                } 
            }
        }
        else
        {
            if (_forwardDirection.z == 1)
            {
                if (playerDirection.z > 0)
                {
                    return true;
                }
            }
            else
            {
                if (playerDirection.z < 0)
                {
                    return true;
                }
            }
        }
        return false;
    }
    /*
     * Compute interception point of circle and line -> fire at position where player is expected to be next
     * Circle represents fire radius of tower, line represents constant player velocity
     * Circle equation: (interceptionPoint - transform.Position)^2 = (projectileSpeed * time)^2
     * Line equation: interceptionPoint = _currentPlayerPosition + (playerVelocity * time)
     * Use line equation in circle equation and solve for time variable
     */
    private Vector3 PredictPlayerPosition(Vector3 playerDirection)
    {
        Vector3 playerVelocity = (_currentPlayerPosition - _previousPlayerPosition) * (1 / Time.deltaTime); //unit per second

        if (playerVelocity.sqrMagnitude == 0)
        {
            return _currentPlayerPosition;
        }

        float a = Vector3.Dot(playerVelocity, playerVelocity) - (_projectileSpeed * _projectileSpeed);
        float b = 2 * Vector3.Dot(playerVelocity, playerDirection);
        float c = Vector3.Dot(playerDirection, playerDirection);
        float discriminant = (b * b) - (4 * a * c);

        if (discriminant < 0)
        {
            return _currentPlayerPosition;
        }

        float squaredDiscriminant = Mathf.Sqrt(discriminant);
        float divisor = 2 * a;
        float time1 = (-b + squaredDiscriminant) / divisor;
        float time2 = (-b - squaredDiscriminant) / divisor;
        float time = ((time1 > 0) && (time2 > 0)) ? Mathf.Min(time1, time2) : Mathf.Max(time1, time2);

        if (time < 0)
        {
            return _currentPlayerPosition;
        }

        Vector3 predictedPosition = _currentPlayerPosition + (playerVelocity * time);
        return predictedPosition;
    }
}
