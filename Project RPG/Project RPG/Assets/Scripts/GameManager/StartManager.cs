using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartManager : MonoBehaviour
{
    public void OnClickGameStart()
    {
        LoadingSceneManager.LoadScene("Village");
    }

    public void OnClickGameExit()
    {
        Application.Quit();
    }
}
