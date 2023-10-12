using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum Lane
{
    Top,
    Middle,
    Bottom
}

[RequireComponent(typeof(Animator))]
public class AnimateOnNoteHit : MonoBehaviour
{
    [SerializeField] private Lane laneHitToAnimate;

    private void OnEnable()
    {
        switch (laneHitToAnimate)
        {
            case Lane.Top:
                LaneManager.AnimateAllTopLanes += AnimateObject;
                break;
            case Lane.Middle:
                LaneManager.AnimateAllMiddleLanes += AnimateObject;
                break;
            case Lane.Bottom:
                LaneManager.AnimateAllBottomLanes += AnimateObject;
                break;
        }
    }

    private void OnDisable()
    {
        switch (laneHitToAnimate)
        {
            case Lane.Top:
                LaneManager.AnimateAllTopLanes -= AnimateObject;
                break;
            case Lane.Middle:
                LaneManager.AnimateAllMiddleLanes -= AnimateObject;
                break;
            case Lane.Bottom:
                LaneManager.AnimateAllBottomLanes -= AnimateObject;
                break;
        }
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
