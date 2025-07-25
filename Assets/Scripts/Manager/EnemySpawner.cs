using UnityEngine;
using System.Collections.Generic;
using System;

public class EnemySpawner : SingletonMonoBehaviour<EnemySpawner>
{
    public event Action OnEveryEnemyDied;
    public event Action<int> OnEnemyCountChanged;
    [Header("References")]
    [SerializeField] private List<Enemy> enemyPrefabList;
    [SerializeField] private List<Transform> spawnPointList;
    [Header("Settings")]
    [SerializeField] private float spawnInterval = 2;
    private List<Enemy> spawnedEnemyList = new List<Enemy>();

    private int enemyToSpawn;
    private int enemySpawned;
    private float spawnTimer;
    private bool isSpawning;
    private int currentEnemyCount;

    private void Update()
    {
        if (!isSpawning)
        {
            return;
        }
        spawnTimer -= Time.deltaTime;
        if (spawnTimer <= 0 && enemySpawned < enemyToSpawn)
        {
            SpawnEnemy();
            spawnTimer = spawnInterval;
        }
        CheckAllEnemiesDied();
    }

    public void StartSpawning(int totalEnemy)
    {
        enemyToSpawn = TurnManager.Instance.GetEnemyForCurrentTurn();
        enemySpawned = 0;
        spawnTimer = spawnInterval;
        isSpawning = true;
        spawnedEnemyList.Clear();
        SetEnemyCount(enemyToSpawn);
    }

    private void SpawnEnemy()
    {
        int randomEnemyIndex = UnityEngine.Random.Range(0, enemyPrefabList.Count);
        int randomSpawnPointIndex = UnityEngine.Random.Range(0, spawnPointList.Count);
        Enemy enemy = Instantiate(enemyPrefabList[randomEnemyIndex], spawnPointList[randomSpawnPointIndex].position, Quaternion.identity);
        spawnedEnemyList.Add(enemy);
        enemySpawned++;
    }

    private void CheckAllEnemiesDied()
    {
        spawnedEnemyList.RemoveAll(enemy => enemy == null);
        if (isSpawning && enemySpawned == enemyToSpawn && spawnedEnemyList.Count == 0)
        {
            isSpawning = false;
            OnEveryEnemyDied?.Invoke();
        }
    }

    public void SetEnemyCount(int count)
    {
        currentEnemyCount = count;
        OnEnemyCountChanged?.Invoke(currentEnemyCount);
    }

    public int GetCurrentEnemyCount()
    {
        return currentEnemyCount;
    }
}
