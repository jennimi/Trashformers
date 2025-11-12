using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour
{
    [System.Serializable]
    public class Wave
    {
        public string waveName;
        // Total number of kill target
        public int killTarget;
        // Interval at which to spawn enemies
        public float spawnInterval;
        // Number of enemy groups in this wave
        public List<EnemyGroup> enemyGroups;
        [HideInInspector] public int killCount = 0;
    }

    [System.Serializable]
    public class EnemyGroup
    {
        public string enemyName;
        // Frequency to spawn this enemy
        public int weight;
        // Number of enemies spawned at once
        public int spawnAtOnce;
        public GameObject enemy;
    }

    // A list of waves
    public List<Wave> waves;
    public int currentWaveCount;
    public float waveInterval;
    Transform player;

    [Header("Spawner Attributes")]
    float spawnTimer;

    void Start()
    {
        player = FindAnyObjectByType<PlayerController>().transform;
    }

    // Update is called once per frame
    void Update()
    {
        // Check  whether a wave starts or ends
        if (currentWaveCount < waves.Count && waves[currentWaveCount].killCount >= waves[currentWaveCount].killTarget)
        {
            StartCoroutine(StartNextWave());
        }

        spawnTimer += Time.deltaTime;

        // Check time to spawn enemy
        if (spawnTimer >= waves[currentWaveCount].spawnInterval)
        {
            spawnTimer = 0f;
            SpawnEnemies();
        }
    }

    IEnumerator StartNextWave()
    {
        GameObject[] remainingEnemies = GameObject.FindGameObjectsWithTag("Enemy");

        foreach (GameObject enemy in remainingEnemies)
        {
            Destroy(enemy);
        }

        yield return new WaitForSeconds(waveInterval);

        if (currentWaveCount < waves.Count - 1)
        {
            currentWaveCount++;
        }
    }
    void SpawnEnemies()
    {
        var wave = waves[currentWaveCount];

        if (wave.killCount >= wave.killTarget) return;

        int totalWeight = 0;
        foreach (var group in wave.enemyGroups)
            totalWeight += group.weight;

        int randomValue = Random.Range(0, totalWeight);
        EnemyGroup selectedGroup = null;

        foreach (var group in wave.enemyGroups)
        {
            if (randomValue < group.weight)
            {
                selectedGroup = group;
                break;
            }
            randomValue -= group.weight;
        }

        for (int i = 0; i < selectedGroup.spawnAtOnce; i++)
        {
            Vector2 spawnPosition = new Vector2(player.transform.position.x + Random.Range(-10f, 10f), player.transform.position.y + Random.Range(-10f, 10f));
            GameObject enemy = Instantiate(selectedGroup.enemy, spawnPosition, Quaternion.identity);

            var enemyStats = enemy.GetComponent<EnemyStats>();
            if (enemyStats != null)
            {
                enemyStats.spawner = this;
                enemyStats.waveIndex = currentWaveCount;
            }

            // 👇 Add XP drop assignment here
            var dropManager = enemy.GetComponent<DropRateManager>();
            if (dropManager != null)
            {
                dropManager.xpAmount = GetXPForWave(currentWaveCount);
            }
        }
    }

    public void OnEnemyKilled(int waveIndex)
    {
        if (waveIndex < waves.Count)
        {
            waves[waveIndex].killCount++;
        }
    }

    int GetXPForWave(int wave)
    {
        switch (wave)
        {
            // Wave starts with the index
            case 0: return 10;
            case 1: return 30;
            case 2: return 50;
            case 3: return 70;
            case 4: return 100;
            default: return 10;
        }
    }
}
