    using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject menuScreen;
    
    // [SerializeField]
    // private GameObject levelSelect;

    // public bool levelSelectEnabled = false;
    
    private ScreenWipe screenWipe;
    private TransitionManager transitionLevel;

    // public Image loadingBarFill;


    // public void PlayGame()
    // {
    //     // Change this to add more levels
    //     SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    // }
    private void Awake()
    {
        transitionLevel = FindObjectOfType<TransitionManager>();
    }

    private void Start()
    {
        // levelSelectEnabled = false;
        AudioManager.Instance.PlayMusic("MenuMusic");
    }

    // private void Update()
    // {
    //     levelSelectEnabled = false;
    //     EnableLevelSelect();
    // }
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
        transitionLevel.StartLoad(sceneID);
    }

    public void LoadRandScene()
    {
        int index = Random.Range(1,4);
        transitionLevel.StartLoad(index);
    }

    // public void EnableLevelSelect()
    // {
    //     if(levelSelectEnabled && levelSelect != null)
    //     {
    //         // levelSelectEnabled = true;
    //         levelSelect.SetActive(true);
    //     }    
    // }
    // public void Reload()
    // {
    //     SceneManager.LoadScene(0);
    //     if(levelSelect != null)
    //     {
    //         // levelSelectEnabled = true;
    //         levelSelect.SetActive(true);
    //     }
    // }

}
