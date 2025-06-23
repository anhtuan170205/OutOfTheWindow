using UnityEngine;
using System.Collections.Generic;

public class EnemySpawner : SingletonMonoBehaviour<EnemySpawner>
{
    [SerializeField] private List<Enemy> enemyPrefabList;
    [SerializeField] private List<Transform> spawnPointList;
    [SerializeField] private float spawnInterval = 2;

    private float spawnTimer;

    private void Start()
    {
        spawnTimer = spawnInterval;
    }

    private void Update()
    {
        spawnTimer -= Time.deltaTime;
        if (spawnTimer <= 0)
        {
            SpawnEnemy();
            spawnTimer = spawnInterval;
        }
    }

    private void SpawnEnemy()
    {
        int randomEnemyIndex = Random.Range(0, enemyPrefabList.Count);
        int randomSpawnPointIndex = Random.Range(0, spawnPointList.Count);
        Enemy enemy = Instantiate(enemyPrefabList[randomEnemyIndex], spawnPointList[randomSpawnPointIndex].position, Quaternion.identity);
    }
}
