using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroParticleController : MonoBehaviour
{
    public static IntroParticleController Instance;
    private List<RailPath> railPaths;
    [SerializeField] private RailPath introRail;

    [SerializeField] private int startIndex = 4;

    private RailPath currentRail;
    public List<Vector3> railPositions;

    public int lastChildIndex { get; private set; }

    Vector3 currentRailPos;
    Vector3 startPos;
    float currentLerpPos;

    private float railDistance;
    private void OnEnable()
    {
        ScreenWipe.ScreenWipeFinished += BeginIntro;
    }

    private void OnDisable()
    {
        ScreenWipe.ScreenWipeFinished -= BeginIntro;
    }
    void Start()
    {
        lastChildIndex = startIndex;
        currentRail = introRail;
        railPositions = currentRail.GetRailPath();
        startPos = railPositions[lastChildIndex];
        transform.position = startPos;

        railPaths = new List<RailPath>();
        railPaths.Add(introRail);
        //InvokeRepeating(nameof(SwitchRail), 0f, 1f);
    }

    public void BeginIntro()
    {
        if (railPositions.Count <= 0) return;

        if (currentLerpPos == 0)
            railDistance = Vector3.Distance(railPositions[lastChildIndex], railPositions[lastChildIndex + 1]);

        currentLerpPos += Time.deltaTime;
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
    
}
