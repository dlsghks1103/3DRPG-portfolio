using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartManager : MonoBehaviour
{
    #region Unity Methods
    public void OnClickGameStart()
    {
        LoadingSceneManager.LoadScene("Village");
    }

    public void OnClickGameExit()
    {
        Application.Quit();
    }
    #endregion Unity Methods
}
