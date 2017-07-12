using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

/*
 * Pop up message box if player press new gave while he/she have already played before.
 */
public class CheckGameProgress : MonoBehaviour
{
    public void CheckGameProgressOnClick()
    {
        if(GameManager.Seeds.Count > 0)
        {
            GameObject warningPanel = GameObject.Find("MainMenuCanvas").transform.Find("WarningPanel").gameObject;
            warningPanel.SetActive(true);
        }
        else
        {
            GameManager.State = ApplicationState.NewGame;
            SceneManager.LoadScene("LevelInfinity", LoadSceneMode.Single);
        }
    }
}

