using UnityEngine;
using System.Collections;

/*
 * Move projectile in specific direction using force.
 */
public class ProjectileMotion : MonoBehaviour 
{
    private Vector3 _direction;

    public float speed;

    public Vector3 Direction
    {
        set
        {
            _direction = value;
        }
    }

    void OnEnable()
    {
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        Vector3 velocity = speed * _direction;
        GetComponent<Rigidbody>().AddForce(velocity, ForceMode.VelocityChange);
    }
}
