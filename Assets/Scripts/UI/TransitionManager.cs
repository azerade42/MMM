using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class TransitionManager : MonoBehaviour
{
    [SerializeField] 
    private ScreenWipe screenWipe;

    public static Action LoadLevelStart;
    public static Action LoadLevelComplete;
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        // FindObjectOfType<ScreenWipe>();
    }

    public void StartLoad(int sceneID)
    {
        AudioManager.Instance.musicSource.Stop();
        LoadLevelStart?.Invoke();
        //levelSelectEnabled = false;
        StartCoroutine(LoadLevel(sceneID));
    }

    private IEnumerator LoadLevel(int sceneID)
    {
        screenWipe.ToggleWipe(true);

        while (!screenWipe.isDone)
            yield return null;

        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneID);

        while (!operation.isDone)
            yield return null;

        // FAKE LOAD
        float curTime = 0;
        float totalTime = 0f;

        while (curTime < totalTime)
        {
            curTime += Time.deltaTime;
            yield return null;
        }

        LoadLevelComplete?.Invoke();

        // screenWipe.ToggleWipe(false);
    }
}
