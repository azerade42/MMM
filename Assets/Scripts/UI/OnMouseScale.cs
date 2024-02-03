using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnMouseScale : MonoBehaviour
{
    private Vector3 currentScale;
    [SerializeField]
    float scaleBy = 1.1f;
    // [SerializeField]
    // float idleScaleSpeed = 3f;

    // private bool keepScaling;
    void Awake()
    {
        currentScale = transform.localScale;
        // keepScaling = true;

        // StartCoroutine(IdleScale(keepScaling));
    }
    public void PointerEnter()
    {
        transform.localScale = currentScale * scaleBy;
    }

    public void PointerExit()
    {
        transform.localScale = currentScale;
    }

    // private IEnumerator IdleScale(bool keepScaling)
    // {
    //     while(keepScaling)
    //     {
    //         currentScale = Vector3.Lerp(transform.localScale, transform.localScale * scaleBy, Time.deltaTime * idleScaleSpeed);
    //         yield return new WaitForSeconds(0.7f);
    //         currentScale = Vector3.Lerp(transform.localScale, transform.localScale * scaleBy, Time.deltaTime * idleScaleSpeed);
    //         yield return new WaitForSeconds(0.7f);
    //     }

    //     yield return null;
    // }
}
