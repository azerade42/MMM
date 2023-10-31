using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionEnemyController : MonoBehaviour
{
    private List<RailPath> railPaths;
    public RailPath topRail;
    public RailPath middleRail;
    public RailPath bottomRail;

    [SerializeField] private int startIndex = 10;

    private RailPath currentRail;
    public List<Vector3> railPositions;

    private int lastChildIndex;

    Vector3 currentRailPos;
    Vector3 startPos;
    float currentLerpPos;

    public float backgroundSpeed;

    private float railDistance;

    
    private bool inputDisabled;

    void Start()
    {
        lastChildIndex = startIndex;
        currentRail = topRail;
        railPositions = currentRail.GetRailPath();
        startPos = railPositions[lastChildIndex];
        transform.position = startPos;

        railPaths = new List<RailPath>();
        railPaths.Add(topRail);
        railPaths.Add(middleRail);
        railPaths.Add(bottomRail);
        SwitchRail();

        //InvokeRepeating(nameof(SwitchRail), 0f, 1f);
    }

    void Update()
    {
        if (railPositions.Count <= 0) return;

        if (currentLerpPos == 0)
            railDistance = Vector3.Distance(railPositions[lastChildIndex], railPositions[lastChildIndex + 1]);

        currentLerpPos += Time.deltaTime * backgroundSpeed;
        Vector3 lastRailPos = railPositions[lastChildIndex];

        if (currentLerpPos < railDistance && lastChildIndex < railPositions.Count - 1)
        {
            Vector3 nextRailPos = railPositions[lastChildIndex + 1];
        
            currentRailPos = Vector3.Lerp(lastRailPos, nextRailPos, currentLerpPos / railDistance);
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
            transform.position = startPos;
            // swap to next rail
        }
    }

    public void SwitchRail()
    {
        currentRail = railPaths[UnityEngine.Random.Range(0, railPaths.Count)];
        railPositions = currentRail.GetRailPath();
    }
    public void SwitchRail(int railIndex)
    {
        currentRail = railPaths[railIndex];
        railPositions = currentRail.GetRailPath();
    }

}
