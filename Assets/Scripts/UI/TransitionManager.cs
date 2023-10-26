using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitionManager : MonoBehaviour
{
    [SerializeField] 
    private ScreenWipe screenWipe;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        // FindObjectOfType<ScreenWipe>();
    }

    public void StartLoad(int sceneID)
    {
        AudioManager.Instance.musicSource.Stop();
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

        screenWipe.ToggleWipe(false);
    }
}
