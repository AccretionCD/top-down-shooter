using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnHandler : MonoBehaviour
{
    [SerializeField] Wave[] waves;
    [SerializeField] EnemyController enemy;

    PlayerController player;
    [SerializeField]float campCheckCooldown = 4f;
    [SerializeField] float campCheckDistance = 2f;
    float timeToCampCheck;
    Vector3 previousPlayerPosition;
    bool camping;
    bool dead;   

    Wave currentWave;
    int currentWaveIndex;

    int enemiesRemaining;
    int enemiesLeftToSpawn;
    float timeLeftToSpawn;

    MapGenerator map;

    public event System.Action<int> OnStartWave;

    void Start()
    {
        player = FindObjectOfType<PlayerController>();

        timeToCampCheck = campCheckCooldown + Time.time;
        previousPlayerPosition = player.transform.position;
        player.OnDeath += OnPlayerDeath;

        map = FindObjectOfType<MapGenerator>();
        StartWave();
    }

    void Update()
    {
        if (!dead)
        {
            if (Time.time > timeToCampCheck)
            {
                timeToCampCheck = campCheckCooldown + Time.time;

                camping = (Vector3.Distance(player.transform.position, previousPlayerPosition) < campCheckDistance);

                previousPlayerPosition = player.transform.position;
            }

            if (enemiesLeftToSpawn > 0 && Time.time > timeLeftToSpawn)
            {
                enemiesLeftToSpawn--;
                timeLeftToSpawn = Time.time + currentWave.enemySpawnDelay;

                StartCoroutine(SpawnEnemy());
            }
        }
    }

    IEnumerator SpawnEnemy()
    {
        float spawnDelay = 1f;
        float spawnSpeed = 4f;

        Transform tile = map.GetRandomOpenTile();

        if (camping)
            tile = map.GetTileFromPosition(player.transform.position);

        Material material = tile.GetComponent<MeshRenderer>().material;

        Color color = material.color;
        Color spawnColor = Color.red;

        float spawnTimer = 0;

        while (spawnTimer < spawnDelay)
        {
            material.color = Color.Lerp(color, spawnColor, Mathf.PingPong(spawnTimer * spawnSpeed, 1));;

            spawnTimer += Time.deltaTime;

            yield return null;
        }

        EnemyController spawnedEnemy = Instantiate(enemy, tile.position + Vector3.up, Quaternion.identity);
        spawnedEnemy.OnDeath += OnEnemyDeath; 
    }

    void OnPlayerDeath()
    {
        dead = true;
    }

    void OnEnemyDeath()
    {
        enemiesRemaining--;

        if (enemiesRemaining == 0)
            StartWave();
    }

    void StartWave()
    {
        currentWaveIndex++;

        if (currentWaveIndex - 1 < waves.Length)
        {
            currentWave = waves[currentWaveIndex - 1];

            enemiesLeftToSpawn = currentWave.enemyCount;
            enemiesRemaining = enemiesLeftToSpawn;
        }

        OnStartWave?.Invoke(currentWaveIndex);

        player.transform.position = map.GetTileFromPosition(Vector3.zero).position + Vector3.up;
    }

    [Serializable]
    public class Wave
    {
        public int enemyCount;
        public float enemySpawnDelay;
    }
}
