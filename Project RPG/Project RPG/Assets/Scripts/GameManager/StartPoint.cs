using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartPoint : MonoBehaviour
{
    #region Variables
    bool isLoad = false;
    #endregion Variables

    #region Unity Methods
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void LateUpdate()
    {
        if (isLoad)
        {
            if (GameObject.Find("PlayerCharacter") != null)
            {
                GameObject gameObject = GameObject.Find("PlayerCharacter");
                gameObject.transform.position = transform.position;

                isLoad = false;
            }
        }
    }
    #endregion Unity Methods

    #region Methods
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        isLoad = true;
    }
    #endregion Methods
}
