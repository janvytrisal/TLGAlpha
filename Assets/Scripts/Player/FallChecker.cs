using UnityEngine;
using System.Collections;

/*
 * Publisher: raise PlayerFell event.
 * Checks if player fell.
 */
public class FallChecker : MonoBehaviour 
{
    private float _currentFallDepth;
    private float _lastY;

    public float fallDepth;

    public delegate void PlayerFellHandler();
    public static event PlayerFellHandler PlayerFell;

    void Start()
    {
        _currentFallDepth = 0;
    }
    void FixedUpdate()
    {
        CheckCurrentFallDepth();
    }

    private void CheckCurrentFallDepth()
    {
        float y = transform.position.y;
        if ((y < _lastY) && (!GetComponent<PlayerMotions>().IsGrounded()))
        {
            _currentFallDepth += (_lastY - y);
            if (_currentFallDepth >= fallDepth)
            {
                if (PlayerFell != null)
                {
                    PlayerFell();
                }
            }
        }
        else
        {
            _currentFallDepth = 0;
        }
        _lastY = y;
    }
}
