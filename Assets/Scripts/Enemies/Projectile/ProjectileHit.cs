using UnityEngine;
using System.Collections;

/*
 * Publisher: raise PlayerHit event.
 */
public class ProjectileHit : MonoBehaviour 
{
    private ProjectileTimer _projectileTimer;

    public delegate void PlayerHitHandler();
    public static event PlayerHitHandler PlayerHit;

    void Start()
    {
        _projectileTimer = GetComponent<ProjectileTimer>();
    }

    void OnTriggerEnter(Collider otherCollider)
    {
        if (otherCollider.gameObject.tag == "Player")
        {
            if (PlayerHit != null)
            {
                PlayerHit();
            }
        }
        _projectileTimer.DeactivateProjectile(); 
    }
}
