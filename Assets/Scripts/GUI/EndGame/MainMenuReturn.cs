using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

/*
 * Loads main menu.
 */
public class MainMenuReturn : MonoBehaviour 
{
    public void ReturnOnClick()
    {
        GameManager.State = ApplicationState.EndGame;
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
    }
}
