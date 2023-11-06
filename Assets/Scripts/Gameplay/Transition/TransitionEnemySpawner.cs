using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.SocialPlatforms.Impl;

public class TransitionEnemySpawner : MonoBehaviour
{
    public static TransitionEnemySpawner Instance;
    public static Action<int> AddScore;
    [SerializeField] private TransitionEnemyController transitionEnemyPrefab;

    [SerializeField] private RailPath topRail;
    [SerializeField] private RailPath middleRail;
    [SerializeField] private RailPath bottomRail;

    public ObjectPool<TransitionEnemyController> objPool;

    [SerializeField] private float levelTime;
    [SerializeField] private float spawnRate;
    [SerializeField] private float speedUpAmount;

    

    // [SerializeField] private ParticleSystem sparkEffect;
    TransitionPlayerController pc;

    private void Awake()
    {
        Instance = this;

        objPool = new ObjectPool<TransitionEnemyController>(CreateEnemy,
        enemy => {
            enemy.gameObject.SetActive(true);
        }, enemy => {
            enemy.gameObject.SetActive(false);
        }, enemy => {
            Destroy(enemy.gameObject);
        }, false,
        20,
        60
        );

    }

    

    private void Start()
    {
        pc = TransitionPlayerController.Instance;
        //CreateEnemy(10);

        AudioManager.Instance.PlayMusic("TransitionMusic");

        StartCoroutine(nameof(SpawnEnemies));
    }

    // Overloaded method for instantiating each individual enemy
    private TransitionEnemyController CreateEnemy()
    {
        TransitionEnemyController enemy = Instantiate(transitionEnemyPrefab, Vector3.one * -100, Quaternion.identity, transform);
        return enemy;
    }

    private void CreateEnemy(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            TransitionEnemyController enemy = objPool.Get();
            
            enemy.gameObject.SetActive(false);
        }
    }

    private void SpawnEnemy()
    {
        if (pc.lastChildIndex + 50 > pc.railPositions.Count - 1) return;

        TransitionEnemyController enemy = objPool.Get();
        enemy.topRail = topRail;
        enemy.middleRail = middleRail;
        enemy.bottomRail = bottomRail;
    }

    private IEnumerator SpawnEnemies()
    {
        float curTime = 0;
        float timeSinceSpawn = 0;
        float timeSinceScore = 0;
        float scoreTimeReset = 0.5f;
        while (curTime < levelTime)
        {
            curTime += Time.deltaTime;
            timeSinceSpawn += Time.deltaTime;
            timeSinceScore += Time.deltaTime;
            if (timeSinceScore > scoreTimeReset)
            {
                timeSinceScore = 0;
                AddScore?.Invoke(4);
            }
            if (timeSinceSpawn > 1/spawnRate)
            {
                TransitionPlayerController.Instance.backgroundSpeed += speedUpAmount;
                scoreTimeReset -= speedUpAmount/100f;

                
                timeSinceSpawn = 0;
                SpawnEnemy();
            }
            yield return null;
        }
        Win();
    }

    private void Lose()
    {

    }

    private void Win()
    {
        print("Win");
    }
}
