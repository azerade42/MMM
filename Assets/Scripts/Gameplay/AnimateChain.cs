using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class AnimateOnChain : MonoBehaviour
{
    private bool firstChain = false;
    private void OnEnable()
    {
        LaneManager.HitPerfect += AnimateObject;
    }

    private void OnDisable()
    {
        LaneManager.HitPerfect -= AnimateObject;
    }

    private void AnimateObject()
    {
        if (!firstChain)
        {
            firstChain = true;
            GetComponent<Animator>().SetTrigger("BreakFirstChain");
        }
        else
        {
            GetComponent<Animator>().SetTrigger("BreakSecondChain");
        }
    }

    public void ResetAnimTrigger()
    {
        GetComponent<Animator>().ResetTrigger("Animate");
    }
}
