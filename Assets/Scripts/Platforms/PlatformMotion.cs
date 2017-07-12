using UnityEngine;
using System.Collections;

/*
 * Base class for platform motion. 
 * Platform is moving with "speed".
 * Platform will move "travelDistance" units, then return.
 * GetMotionCenter and GetMotionExtents are used to create placeholders around platform. 
 * Method MoveAll move platform and player.
 */
public abstract class PlatformMotion : MonoBehaviour
{
    protected GameObject _player;
    protected bool _destinationReached;
    protected Vector3 _startingPosition;

    public float speed;
    public float travelDistance; 

    public GameObject Player
    {
        set
        {
            _player = value;
        }
    }
    public bool DestinationReached
    {
        get
        {
            return _destinationReached;
        }
        set
        {
            _destinationReached = value;
        }
    }
    public Vector3 StartingPosition
    {
        get
        {
            return _startingPosition;
        }
        set
        {
            _startingPosition = value;
        }
    }

    public abstract Vector3 GetDestinationPosition();
    public abstract void ShiftToDestinationPosition();
    public abstract Vector3 GetMotionCenter();
    public abstract Vector3 GetMotionExtents();

    protected virtual void MoveAll(Vector3 velocity)
    { 
        if (velocity.y > 0)
        {
            if (_player != null)
            {
                Vector3 rotatedVelocity = transform.rotation * velocity;
                _player.GetComponent<CharacterController>().Move(rotatedVelocity);
            }
            transform.Translate(velocity);
        }
        else
        {
            transform.Translate(velocity);  
            if (_player != null)
            {
                Vector3 rotatedVelocity = transform.rotation * velocity;
                _player.GetComponent<CharacterController>().Move(rotatedVelocity);
            }          
        }
    }
}
