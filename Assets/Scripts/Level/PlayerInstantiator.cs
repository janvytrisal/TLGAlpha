using UnityEngine;
using System.Collections;

public class PlayerInstantiator : MonoBehaviour 
{
    public GameObject playerPrefab;

    public void InstantiatePlayer()
    {
        GameManager.Player = Instantiate(playerPrefab);
    }
}
