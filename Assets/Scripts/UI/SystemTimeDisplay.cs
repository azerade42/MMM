using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public class SystemTimeDisplay : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI clockText;

    void Start()
    {
        InvokeRepeating("StartClock", 0f, 1.0f);
    }
    void StartClock()
    {
        clockText.text = System.DateTime.Now.ToString("hh:mm tt");
    }
}
