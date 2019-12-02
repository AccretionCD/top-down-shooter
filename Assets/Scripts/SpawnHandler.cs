using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnHandler : MonoBehaviour
{
    [SerializeField] Wave[] waves;
    [SerializeField] EnemyController enemy;

    Wave currentWave;
    int currentWaveIndex;

    int enemiesRemaining;
    int enemiesLeftToSpawn;
    float timeLeftToSpawn;

    void Start()
    {
        StartWave();
    }

    void Update()
    {
        if (enemiesLeftToSpawn > 0 && Time.time > timeLeftToSpawn)
        {
            enemiesLeftToSpawn--;
            timeLeftToSpawn = Time.time + currentWave.enemySpawnDelay;

            EnemyController spawnedEnemy = Instantiate(enemy, Vector3.zero, Quaternion.identity);
            spawnedEnemy.OnDeath += OnEnemyDeath;
        }
    }

    void OnEnemyDeath()
    {
        enemiesRemaining--;

        if (enemiesRemaining == 0)
            StartWave();
    }

    void StartWave()
    {
        if (currentWaveIndex < waves.Length)
        {
            currentWave = waves[currentWaveIndex];

            enemiesLeftToSpawn = currentWave.enemyCount;
            enemiesRemaining = enemiesLeftToSpawn;
        }

        currentWaveIndex++;
    }

    [Serializable]
    public class Wave
    {
        public int enemyCount;
        public float enemySpawnDelay;
    }
}
