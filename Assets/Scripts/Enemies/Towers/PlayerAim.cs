using UnityEngine;
using System.Collections;

/*
 * Aim at player when he/she is in front of tower and nothing block view.
 * Setting ClearView property.
 */
public class PlayerAim : MonoBehaviour 
{
    private Vector3 _playerPosition;
    private bool _clearView;
    private float _aimDistanceSquared; //performance optimisation
    private Vector3 _forwardDirection;

    public float aimDistance;

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
    }
    void FixedUpdate()//Update()
    {
        UpdatePlayerPosition();
        LookAtPlayer();
    }

    private void UpdatePlayerPosition()
    {
        GameObject player = GameManager.Player;
        if (player != null)
        {
            _playerPosition = player.transform.position;
        }
    }
    private void LookAtPlayer()
    {
        Vector3 playerDirection = _playerPosition - transform.position;
        if (PlayerInFront(playerDirection) && (playerDirection.sqrMagnitude < _aimDistanceSquared))
        {
            if (!_clearView) //to lessen Raycasts
            {
                _clearView = ClearViewCheck(playerDirection);
            }
            if (_clearView)
            {
                Vector3 lookAtOffset = new Vector3(0, GameManager.Player.transform.lossyScale.y / 3, 0); //to aim above body center
                transform.LookAt(_playerPosition + lookAtOffset);
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
        Debug.DrawRay(transform.position, playerDirection.normalized, Color.yellow);
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
}
