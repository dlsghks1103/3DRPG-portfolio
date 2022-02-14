using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadingSceneManager : MonoBehaviour
{
    #region Variables
    public static string nextScene;

    public Slider slider;
    #endregion Variables

    #region Unity Methods
    void Start()
    {
        StartCoroutine(LoadScene());
    }
    #endregion Unity Methods

    #region Methods
    public static void LoadScene(string sceneName)
    {
        nextScene = sceneName;
        SceneManager.LoadScene("LoadingScene");
    }

    IEnumerator LoadScene()
    {
        yield return null;
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(nextScene);
        asyncOperation.allowSceneActivation = false;
        float timer = 0.0f;
        while (!asyncOperation.isDone)
        {
            yield return null;
            timer += Time.deltaTime;
            if (asyncOperation.progress < 0.9f)
            {
                slider.value = Mathf.Lerp(slider.value, asyncOperation.progress, timer);
                if (slider.value >= asyncOperation.progress)
                {
                    timer = 0f;
                }
            }
            else
            { 
                slider.value = Mathf.Lerp(slider.value, 1f, timer);
                if (slider.value == 1.0f)
                {
                    asyncOperation.allowSceneActivation = true;
                    yield break;
                }
            }
        }
    }
    #endregion Methods
}
