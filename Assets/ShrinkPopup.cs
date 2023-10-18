using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShrinkPopup : MonoBehaviour
{
    private RectTransform rt;

    [SerializeField] private int numTimesUntilDeleted;

    private Vector3 startScale;

    private void Start()
    {
        rt = GetComponent<RectTransform>();
        startScale = rt.localScale;
    }
    public void ShrinkThatPopup()
    {
        rt.localScale -= Vector3.one / numTimesUntilDeleted;
        if (rt.localScale.x <= 0)
        {
            gameObject.SetActive(false);
            rt.localScale = startScale;
        }
    }
}
