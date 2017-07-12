using UnityEngine;
using System.Collections;

/*
 * Turning on/off immortality shield.
 */
public class Immortality : MonoBehaviour 
{
    private float timeLeft;

    public float duration;
    public GameObject shield;

    void OnEnable() 
    {
        timeLeft = duration;
        shield.SetActive(true);
    }
    void Update()
    {
        timeLeft -= Time.deltaTime;
        if (timeLeft <= 0)
        {
            shield.SetActive(false);
            enabled = false;
        }
    }
}
