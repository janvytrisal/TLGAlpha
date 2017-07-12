using UnityEngine;
using System.Collections;

/*
 * Stops fast moving player to jump throught moving platform. Set player's y velocity to zero when player enter the trigger.
 */
public class PlayerStopper : MonoBehaviour 
{
    void OnTriggerEnter(Collider otherCollider)
    {
        if (otherCollider.gameObject.tag == "Player")
        {
            PlayerMotions playerMotions = otherCollider.gameObject.GetComponent<PlayerMotions>();
            Vector3 oldVelocity = playerMotions.Velocity;
            playerMotions.Velocity = new Vector3(oldVelocity.x, 0, oldVelocity.z);
        }
    }
}
