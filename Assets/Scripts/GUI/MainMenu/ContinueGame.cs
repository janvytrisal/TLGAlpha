using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

/*
 * Loads last played level.
 */
public class ContinueGame : MonoBehaviour 
{
    public void ContinueOnClick()
    {
        GameManager.State = ApplicationState.Continue;
        SceneManager.LoadScene("LevelInfinity", LoadSceneMode.Single);
    }
}
