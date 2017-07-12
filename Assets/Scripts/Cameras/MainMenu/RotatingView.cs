using UnityEngine;
using System.Collections;

/*
 * Rotate Main Camera in slow motion.
 */
public class RotatingView : MonoBehaviour 
{
    void Update()
    {
        Vector3 rotation = Vector3.one;
        rotation *= (Time.deltaTime / 2);
        transform.Rotate(rotation);
    }
}
