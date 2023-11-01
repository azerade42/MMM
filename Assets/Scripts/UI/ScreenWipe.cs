using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class ScreenWipe : MonoBehaviour
{
    public static Action ScreenWipeFinished;

    [SerializeField]
    [Range(0.1f, 5f)]
    private float wipeSpeed = 1f;

    [SerializeField]
    private Image background;

    [SerializeField]
    private Image loadingIcon;

    // private RectTransform loadingIconRT;

    [SerializeField]
    private RectTransform startPos;
    [SerializeField]
    private RectTransform endPos;

    private enum WipeMode {NotBlocked, WipingToNotBlocked, Blocked, WipingToBlocked}
    
    private WipeMode wipeMode = WipeMode.NotBlocked;

    private float wipeProgress;

    public bool isDone {get; private set;}

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        background.gameObject.SetActive(false);
        // loadingIconRT = loadingIcon.gameObject.GetComponent<RectTransform>();
        // loadingIcon.gameObject.GetComponent<FadeScript>();
    }

    private void OnEnable()
    {
        TransitionManager.LoadLevelStart += NickWipeToBlocked;
        FadeScript.FadeOutComplete += NickWipeToNotBlocked;
    }

    private void OnDisable()
    {
        TransitionManager.LoadLevelStart -= NickWipeToBlocked;
        FadeScript.FadeOutComplete -= NickWipeToNotBlocked;
    }

    private void NickWipeToBlocked() => ToggleWipe(true);

    private void NickWipeToNotBlocked() => ToggleWipe(false);

    public void ToggleWipe(bool blockScreen)
    {
        isDone = false;
        if (blockScreen)
            wipeMode = WipeMode.WipingToBlocked;
        else   
            wipeMode = WipeMode.WipingToNotBlocked;
    }

    private void Update()
    {
        switch(wipeMode)
        {
            case WipeMode.WipingToBlocked:
                WipeToBlocked();
                break;
            case WipeMode.WipingToNotBlocked:
                WipeToNotBlocked();
                break;
        }
    }

    private void WipeToBlocked()
    {
        background.gameObject.SetActive(true);
        wipeProgress += Time.deltaTime * (1f / wipeSpeed);
        // loadingIconRT.anchoredPosition = Vector2.Lerp(startPos.anchoredPosition, endPos.anchoredPosition, wipeProgress);

        background.fillAmount = wipeProgress;
        if (wipeProgress >= 1f)
        {
            isDone = true;
            wipeMode = WipeMode.Blocked;

            loadingIcon.gameObject.SetActive(true);

            ScreenWipeFinished?.Invoke();
        }
    }

    private void WipeToNotBlocked()
    {
        wipeProgress -= Time.deltaTime * (1f / wipeSpeed);
        background.fillAmount = wipeProgress;
        // loadingIconRT.anchoredPosition = Vector2.Lerp(startPos.anchoredPosition, endPos.anchoredPosition, wipeProgress);


        if (wipeProgress <= 0)
        {
            isDone = true;
            wipeMode = WipeMode.NotBlocked;
        }
        if (isDone)
            background.gameObject.SetActive(false);

    }

    [ContextMenu("Block")]
    private void Block() {ToggleWipe(true);}
    [ContextMenu("Clear")]
    private void Clear() {ToggleWipe(false);}

}
