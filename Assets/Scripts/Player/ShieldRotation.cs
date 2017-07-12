using UnityEngine;
using System.Collections;

/*
 * Rotate shield around y axis.
 */
public class ShieldRotation : MonoBehaviour 
{
    public float speed;

    void Update()
    {
        Vector3 rotation = Vector3.up;
        rotation *= (speed * Time.deltaTime);
        transform.Rotate(rotation);
    }
}
