using UnityEngine;
using System.Collections;

/*
 * Displays game data.
 */
public class OnGUIMain : MonoBehaviour 
{
    void OnGUI()
    {
        GUI.Label(new Rect(10, 475, 500, 25), "Level number: " + GameManager.Seeds.Count);
        GUI.Label(new Rect(10, 500, 500, 25), "Level identificator: " + GameManager.Seeds[GameManager.Seeds.Count - 1]);
        GUI.Label(new Rect(10, 575, 500, 25), "Deaths: " + GameManager.DeathCount);
        GUI.Label(new Rect(10, 600, 500, 25), "Highscore: " + GameManager.Highscore);
        GUI.Label(new Rect(10, 625, 500, 200), "Seedlist: " + SeedList());
    }
    private string SeedList()
    {
        string seedList = "";
        foreach(int seed in GameManager.Seeds)
        {
            seedList += seed + ", ";
        }
        return seedList;
	}
}
