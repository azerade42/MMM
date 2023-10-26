using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TransitionPlayerController : MonoBehaviour
{
    [SerializeField] private RailPath topRail;

    private RailPath currentRail;
    public List<Vector3> railPositions;

    int lastChildIndex = 0;

    Vector3 currentRailPos;
    float currentLerpPos;

    void Start()
    {
        currentRail = topRail;
        railPositions = currentRail.GetRailPath();
    }

    void Update()
    {
        if (railPositions.Count <= 0) return;

        currentLerpPos += Time.deltaTime; // multiplied by the speed of the background
        Vector3 lastRailPos = railPositions[lastChildIndex];

        if (currentLerpPos < 1 && lastChildIndex < railPositions.Count - 1)
        {
            Vector3 nextRailPos = railPositions[lastChildIndex + 1];
        
            currentRailPos = Vector3.Lerp(lastRailPos, nextRailPos, currentLerpPos);
            transform.position = currentRailPos;

        }
        else if (currentLerpPos >= 1)
        {
            lastChildIndex++;
            currentLerpPos = 0;
        }

        if (lastChildIndex >= railPositions.Count - 1)
        {
            lastChildIndex = 0;
            // swap to next rail
        }
    }
}
