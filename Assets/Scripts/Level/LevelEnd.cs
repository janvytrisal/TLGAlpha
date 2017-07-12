using UnityEngine;
using System.Collections;

/*
 * Publisher: raise LevelEndReached event.
 */
public class LevelEnd : MonoBehaviour 
{
    public delegate void LevelEndReachedHandler();
    public static event LevelEndReachedHandler LevelEndReached;

    void OnTriggerEnter(Collider otherCollider)
    {
        if (otherCollider.gameObject.tag == "Player")
        {
            if (LevelEndReached != null)
            {
                LevelEndReached();
            }
        }
    }
}
