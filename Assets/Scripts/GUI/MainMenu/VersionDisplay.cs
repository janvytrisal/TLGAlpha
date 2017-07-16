using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class VersionDisplay : MonoBehaviour
{
    void Start()
    {
        GetComponent<Text>().text = "Ver. " + Application.version;
    }
}

