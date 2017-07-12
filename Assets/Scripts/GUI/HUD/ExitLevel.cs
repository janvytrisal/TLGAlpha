using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

/*
 * Return to main menu.
 */
public class ExitLevel : MonoBehaviour 
{
    public void ExitOnClick()
    {
        GameManager.State = ApplicationState.ExitLevel;
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
    }
}
