using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TransitionPlayerController : MonoBehaviour
{
    [SerializeField] private RailPath topRail;
    [SerializeField] private ScreenMoveLeft screenMoveLeft;

    private RailPath currentRail;
    public List<Vector3> railPositions;

    int lastChildIndex = 3;

    Vector3 currentRailPos;
    Vector3 startPos;
    float currentLerpPos;

    float backgroundSpeed;

    void Start()
    {
        currentRail = topRail;
        railPositions = currentRail.GetRailPath();
        startPos = railPositions[3];
        transform.position = startPos;
        backgroundSpeed = screenMoveLeft.speed;
    }

    void Update()
    {
        if (railPositions.Count <= 0) return;

        currentLerpPos += Time.deltaTime * backgroundSpeed * 0.2f;
        Vector3 lastRailPos = railPositions[lastChildIndex];

        if (currentLerpPos < 1 && lastChildIndex < railPositions.Count - 1)
        {
            Vector3 nextRailPos = railPositions[lastChildIndex + 1];
        
            currentRailPos = Vector3.Lerp(lastRailPos, nextRailPos, currentLerpPos);
            transform.position = new Vector3(startPos.x, currentRailPos.y, currentRailPos.z);

        }
        else if (currentLerpPos >= 1)
        {
            lastChildIndex++;
            currentLerpPos = 0;
        }

        if (lastChildIndex >= railPositions.Count - 1)
        {
            lastChildIndex = 0;
            transform.position = startPos;
            // swap to next rail
        }
    }
}
