using UnityEngine;
using System.Collections.Generic;
using System;

public class EnemySpawner : MonoBehaviour
{
    public event Action OnEveryEnemyDied;
    public event Action<int> OnEnemyCountChanged;

    [Header("References")]
    [SerializeField] private List<Enemy> enemyPrefabList;
    [SerializeField] private List<Vector3> spawnPointList;

    [Header("Settings")]
    [SerializeField] private float spawnInterval = 2f;

    private List<Enemy> spawnedEnemyList = new List<Enemy>();
    private int enemyToSpawn;
    private int enemySpawned;
    private float spawnTimer;
    private bool isSpawning;
    private int currentEnemyCount;

    private void Update()
    {
        if (!isSpawning)
            return;

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
        enemyToSpawn = totalEnemy;
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

        Vector3 spawnPosition = spawnPointList[randomSpawnPointIndex];
        Enemy enemy = Instantiate(enemyPrefabList[randomEnemyIndex], spawnPosition, Quaternion.identity);

        spawnedEnemyList.Add(enemy);
        enemySpawned++;
        Debug.Log($"Spawned enemy {enemy.name}. Total spawned: {enemySpawned}/{enemyToSpawn}");
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

    public int GetCurrentEnemyCount() => currentEnemyCount;
}
