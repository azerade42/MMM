using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class AnimateOnPerfect : MonoBehaviour
{
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
        GetComponent<Animator>().SetTrigger("Animate");
    }

    public void ResetAnimTrigger()
    {
        GetComponent<Animator>().ResetTrigger("Animate");
    }
}
