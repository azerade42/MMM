using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// [RequireComponent(typeof(Image))]

public class FadeScript : MonoBehaviour
{
    public static Action FadeOutComplete;
    [SerializeField] private float fadeTime;
    // [SerializeField] private float fadeOutime;

    private Image _image;
    private float _opacity
    {
        set
        {
            _image.color = new Color(1,1,1, value);
        }
    }

    private void OnEnable()
    {
        TransitionManager.LoadLevelComplete += BeginFadeOut;
        ScreenWipe.ScreenWipeFinished += BeginFadeIn;
    }

    private void OnDisable()
    {
        TransitionManager.LoadLevelComplete -= BeginFadeOut;
        ScreenWipe.ScreenWipeFinished -= BeginFadeIn;
    }
    private void Awake()
    {
        _image = GetComponent<Image>();
    }

    public void BeginFadeIn()
    {
        StartCoroutine(FadeIn(fadeTime));
    }

    public void BeginFadeOut()
    {
        StartCoroutine(FadeOut(fadeTime));
    }

    private IEnumerator FadeIn(float fadeInTime)
    {
        float elapsedTime = 0f;
        while (elapsedTime < fadeInTime)
        {
            elapsedTime += Time.deltaTime;
            _opacity = elapsedTime/fadeInTime;
            yield return null;
        }
    }

    private IEnumerator FadeOut(float fadeOutTime)
    {
        float elapsedTime = 0f;
        while (elapsedTime < fadeOutTime)
        {
            elapsedTime += Time.deltaTime;
            _opacity = 1 - elapsedTime/fadeOutTime;
            yield return null;
        }

        FadeOutComplete?.Invoke();
    }

}
