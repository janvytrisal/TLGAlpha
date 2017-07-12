using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class StartNewGame : MonoBehaviour 
{
    public void StartOnClick()
    {
        GameManager.State = ApplicationState.NewGame;
        SceneManager.LoadScene("LevelInfinity", LoadSceneMode.Single);
    }
}
