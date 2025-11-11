using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour
{
    [System.Serializable]
    public class Wave
    {
        public string waveName;
        // Total number of enemies to spawn in this wave
        public int waveQuota;
        // Interval at which to spawn enemies
        public float spawnInterval;
        // Number of enemies already spawned in this wave
        public int spawnCount;
        // Number of enemy groups in this wave
        public List<EnemyGroup> enemyGroups;
    }

    [System.Serializable]
    public class EnemyGroup
    {
        public string enemyName;
        // Number of enemies to spawn for this wave
        public int enemyCount;
        // Number of enemies of this type already spawned in this wave
        public int spawnCount;
        public GameObject enemy;
    }

    // A list of waves
    public List<Wave> waves;
    public int currentWaveCount;
    Transform player;

    [Header("Spawner Attributes")]
    float spawnTimer;

    void Start()
    {
        CalculateWaveQuota();
        player = FindObjectOfType<PlayerController>().transform;
    }

    // Update is called once per frame
    void Update()
    {
        spawnTimer += Time.deltaTime;

        // Check time to spawn enemy
        if (spawnTimer >= waves[currentWaveCount].spawnInterval)
        {
            spawnTimer = 0f;
            SpawnEnemies();
        }
    }

    void CalculateWaveQuota()
    {
        int currentWaveQuota = 0;
        foreach (var enemyGroup in waves[currentWaveCount].enemyGroups)
        {
            currentWaveQuota += enemyGroup.enemyCount;
        }

        waves[currentWaveCount].waveQuota = currentWaveQuota;
        Debug.LogWarning(currentWaveQuota);
    }

    void SpawnEnemies()
    {
        // Check if the minimum number of enemies in the wave have been spawned
        if (waves[currentWaveCount].spawnCount < waves[currentWaveCount].waveQuota)
        {
            // Spawn each type of enemy until the quota is filled
            foreach (var enemyGroup in waves[currentWaveCount].enemyGroups)
            {
                // Check if the minimum number of enemies of this type have been spawned
                if (enemyGroup.spawnCount < enemyGroup.enemyCount)
                {
                    Vector2 spawnPosition = new Vector2(player.transform.position.x + Random.Range(-10f, 10f), player.transform.position.y + Random.Range(-10f, 10f));
                    Instantiate(enemyGroup.enemy, spawnPosition, Quaternion.identity);

                    enemyGroup.spawnCount++;
                    waves[currentWaveCount].spawnCount++;
                }
            }
        }
    }
}
