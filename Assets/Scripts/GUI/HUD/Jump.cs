using UnityEngine;
using System.Collections;

/*
 * Start jump action.
 */
public class Jump : MonoBehaviour 
{
    public void JumpOnClick()
    {
        if (GameManager.Player != null)
        {
            GameManager.Player.GetComponent<PlayerMotions>().Jump();
        }
    }
}
