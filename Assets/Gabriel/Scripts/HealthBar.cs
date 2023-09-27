using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider slider;

    public float health;

    public float perfectAdd;
    public float goodAdd;
    public float missSubtract;

    private void Awake()
    {
        perfectAdd = slider.maxValue / 5f;
        goodAdd = slider.maxValue / 10f;
        missSubtract = slider.maxValue / -5f;
    }
    private void OnEnable()
    {
        LaneManager.HitPerfect += PerfectHit;
        LaneManager.HitGood += GoodHit;
        LaneManager.HitMiss += MissHit;
    }

    private void OnDisable()
    {
        LaneManager.HitPerfect -= PerfectHit;
    }

    // public void SetMaxHealth (float health)
    // {
    //     slider.maxValue = health;
    //     slider.value = health;
    // }
    // public void SetHealth (float health)
    // {
    //     slider.value = health;
    // }
    public void PerfectHit()
    {
        float newValue = slider.value + perfectAdd;
        slider.value = Mathf.Clamp(newValue, slider.minValue, slider.maxValue);
    }
    public void GoodHit()
    {
        float newValue = slider.value + goodAdd;
        slider.value = Mathf.Clamp(newValue, slider.minValue, slider.maxValue);
    }
    public void MissHit()
    {
        float newValue = slider.value + missSubtract;
        slider.value = Mathf.Clamp(newValue, slider.minValue, slider.maxValue);

        if (newValue <= 0f)
        {
            //GameManager.Instance.GameOver.Invoke();
        }
    }
}

