using UnityEngine;
using System.Collections;

/*
 * Register player at platform when enter its top trigger.
 * Requeried for MoveAll method in PlatformMotion.
 */
public class PlayerRegister : MonoBehaviour 
{
    private PlatformMotion _parent; //polymorphism

    void Start()
    {
        _parent = GetComponentInParent<PlatformMotion>();
    }

    void OnTriggerEnter(Collider otherCollider)
    {
        if (otherCollider.gameObject.tag == "Player")
        {
            _parent.Player = otherCollider.gameObject;
        }
    }

    void OnTriggerExit(Collider otherCollider)
    {
        if (otherCollider.gameObject.tag == "Player")
        {
            _parent.Player = null;
        }
    }
}
