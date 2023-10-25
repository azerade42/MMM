using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public class SystemTimeDisplay : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI clockText;
    void Update()
    {
        clockText.text = System.DateTime.Now.ToString("hh:mm tt");
    }
}
