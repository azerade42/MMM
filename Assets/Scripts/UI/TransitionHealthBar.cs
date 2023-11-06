using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TransitionHealthBar : MonoBehaviour
{
    public Slider slider;
    public float health;

    [SerializeField] private EndScreen endScreen;

    private void OnEnable()
    {
        TransitionPlayerController.OnHit += TakeDamage;
    }

    private void OnDisable()
    {
        TransitionPlayerController.OnHit -= TakeDamage;
    }

    private void TakeDamage()
    {
        float newValue = slider.value - 1f;
        slider.value = Mathf.Clamp(newValue, slider.minValue, slider.maxValue);


        if (newValue <= 0f)
        {
            endScreen.SetScreenActive();
            GameManager.TriggerGameOver();
            // endScreen.gameObject.SetActive(true);
        }
    }
}
