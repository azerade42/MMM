using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.VisualScripting;
using UnityEngine;
using System;

using UnityEngine.InputSystem;

public class TransitionPlayerController : MonoBehaviour
{
    public static TransitionPlayerController Instance;
    [SerializeField] private ScreenMoveLeft screenMoveLeft;
    private List<RailPath> railPaths;
    [SerializeField] private RailPath topRail;
    [SerializeField] private RailPath middleRail;
    [SerializeField] private RailPath bottomRail;

    [SerializeField] private int startIndex = 4;

    [SerializeField] private SpriteRenderer playerSprite;

    private RailPath currentRail;
    public List<Vector3> railPositions;

    public int lastChildIndex { get; private set; }

    Vector3 currentRailPos;
    Vector3 startPos;
    float currentLerpPos;

    public float backgroundSpeed;

    private float railDistance;
    
    private bool inputDisabled;

    public static Action OnHit;
    public static Action OnUpdateSpeed;

    private bool paused;

    private Collider hurtbox;

    private int dodgeStreak;

    [HideInInspector] public int HighestDodgeStreak;

    void Awake()
    {
        Instance = this;
    }

    void OnEnable()
    {
        GameManager.Pause += () => paused = true;
        GameManager.UnPause += () => paused = false;
        GameManager.GameOver += () => paused = true;
        GameManager.SongOver += () => paused = true;

        TransitionEnemyController.EnemyDied += AddDodgeStreak;
    }

    void OnDisable()
    {
        GameManager.Pause -= () => paused = false;
        GameManager.UnPause -= () => paused = false;
        GameManager.GameOver -= () => paused = true;
        GameManager.SongOver -= () => paused = true;

        
        TransitionEnemyController.EnemyDied -= AddDodgeStreak;

        StopAllCoroutines();
    }

    void Start()
    {
        hurtbox = GetComponent<SphereCollider>();
        lastChildIndex = startIndex;
        currentRail = topRail;
        railPositions = currentRail.GetRailPath();
        startPos = railPositions[lastChildIndex];
        transform.position = startPos;

        railPaths = new List<RailPath>();
        railPaths.Add(topRail);
        railPaths.Add(middleRail);
        railPaths.Add(bottomRail);

        //InvokeRepeating(nameof(SwitchRail), 0f, 1f);
    }

    void Update()
    {
        if (railPositions.Count <= 1) return;
        if (paused) return;

        if (currentLerpPos == 0)
            railDistance = Vector3.Distance(railPositions[lastChildIndex], railPositions[lastChildIndex + 1]);

        currentLerpPos += Time.deltaTime * backgroundSpeed;
        Vector3 lastRailPos = railPositions[lastChildIndex];

        if (currentLerpPos < railDistance && lastChildIndex < railPositions.Count - 1)
        {
            Vector3 nextRailPos = railPositions[lastChildIndex + 1];
        
            currentRailPos = Vector3.Lerp(lastRailPos, nextRailPos, currentLerpPos / railDistance);
            screenMoveLeft.transform.position = new Vector3(-currentRailPos.x, 0, 0);
            transform.position = new Vector3(0, currentRailPos.y, currentRailPos.z);
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
            screenMoveLeft.transform.position = screenMoveLeft.startPos;
            // swap to next rail
        }
    }

    private void SwitchRail()
    {
        currentRail = railPaths[UnityEngine.Random.Range(0, railPaths.Count)];
        railPositions = currentRail.GetRailPath();
    }
    private void SwitchRail(int railIndex)
    {
        currentRail = railPaths[railIndex];
        railPositions = currentRail.GetRailPath();
    }

    public void OnMoveMouse(InputAction.CallbackContext context)
    {
        if (!inputDisabled)
            Move(context.ReadValue<Vector2>());
    }

    private void Move(Vector2 mousePos)
    {
        float curHeight = Screen.height;

        if (mousePos.y > curHeight*2/3f && currentRail != topRail)
        {
            // print("top!");
            SwitchRail(0);
        }
        else if (mousePos.y > curHeight*5/12f && mousePos.y < curHeight*7/12f && currentRail != middleRail)
        {
            // print("middle!");
            SwitchRail(1);
        }
        else if (mousePos.y < curHeight/3f && currentRail != bottomRail)
        {
            // print("bottom!");
            SwitchRail(2);
        }
        else
        {
            // print("WRONG");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<TransitionEnemyController>() != null)
        {
            OnHit?.Invoke();
            dodgeStreak = 0;
            AudioManager.Instance.PlaySFX("MissNote");
            StartCoroutine(PlayerHit());
            StartCoroutine(RampUpSpeed());
        }
    }

    private IEnumerator PlayerHit()
    {
        hurtbox.enabled = false;
        float curTime = 0;
        float blinkTime = 0.25f;
        int timesToBlink = 3;

        for (int i = 0; i < timesToBlink; i++)
        {
            playerSprite.color = Color.red;
            while (curTime < blinkTime / 2)
            {
                curTime += Time.deltaTime;
                yield return null;
            }
            curTime = 0;
            
            playerSprite.color = Color.white;
            while (curTime < blinkTime / 2)
            {
                curTime += Time.deltaTime;
                yield return null;
            }
            curTime = 0;
        }

        hurtbox.enabled = true;
    }

    private IEnumerator RampUpSpeed()
    {
        float endBGSpeed = backgroundSpeed;
        backgroundSpeed /= 2;
        float startBGSpeed = backgroundSpeed;
        float curTime = 0;
        float timeToSpeedUp = 3f;

        while (curTime < timeToSpeedUp)
        {
            curTime += Time.deltaTime;
            backgroundSpeed = Mathf.Lerp(startBGSpeed, endBGSpeed, curTime / timeToSpeedUp);
            OnUpdateSpeed?.Invoke();
            yield return null;
        }
    }
   
    private void AddDodgeStreak() => HighestDodgeStreak = dodgeStreak++ > HighestDodgeStreak ? dodgeStreak : HighestDodgeStreak;
}
