    using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject menuScreen;
    public GameObject loadingScreen;
    public static GameObject levelSelect;
    


    // public Image loadingBarFill;


    // public void PlayGame()
    // {
    //     // Change this to add more levels
    //     SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    // }
    private void Start()
    {
        levelSelect = GameObject.FindWithTag("Level Select");
    }
    public void QuitGame()
    {
        Application.Quit();
    }
    public void MenuLoad()
    {
        SceneManager.LoadScene(0);
    }
    public void OpenMenu()
    {
        menuScreen.SetActive(true);
    }

    public void LoadScene(int sceneID)
    {
        StartCoroutine(LoadSceneAsync(sceneID));
    }
    public void Reload()
    {
        SceneManager.LoadScene(0);
        if(levelSelect != null)
            levelSelect.SetActive(true);
    }

    IEnumerator LoadSceneAsync(int sceneID)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneID);

        loadingScreen.SetActive(true);

        while (!operation.isDone)
        {
            // float progressValue = Mathf.Clamp01(operation.progress / 0.9f);
            // loadingBarFill.fillAmount = progressValue;


            yield return null;
        }
        levelSelect = GameObject.FindWithTag("Level Select");
    }
}
