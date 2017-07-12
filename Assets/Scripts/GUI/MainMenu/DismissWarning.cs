using UnityEngine;
using System.Collections;

public class DismissWarning : MonoBehaviour 
{
    public void DismissWarningOnClick()
    {
        GameObject warningPanel = GameObject.Find("MainMenuCanvas/WarningPanel");
        warningPanel.SetActive(false);
    }
}
