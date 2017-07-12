using UnityEngine;
using System.Collections;

/*
 * Deactive projectile after certain period of time.
 */
public class ProjectileTimer : MonoBehaviour 
{
    private float _timePassed;

    public float deletionTime;

    void Start()
    {
        _timePassed = 0;
    }
    void Update()
    {
        if (_timePassed < deletionTime)
        {
            _timePassed += Time.deltaTime;
        }
        else
        {
            DeactivateProjectile();
        }
    }

    public void DeactivateProjectile()
    {
        transform.position = ProjectileSpawner.defaultProjectilePoolLocation;
        _timePassed = 0;
        gameObject.SetActive(false);
    }
}
