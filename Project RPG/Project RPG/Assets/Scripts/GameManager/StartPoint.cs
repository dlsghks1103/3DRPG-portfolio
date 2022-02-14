using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartPoint : MonoBehaviour
{
    bool isLoad = false;

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        isLoad = true;
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
}
