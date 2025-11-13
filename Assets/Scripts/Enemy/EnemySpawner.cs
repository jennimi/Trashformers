using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour
{
    [System.Serializable]
    public class Wave
    {
        public string waveName;
        public int killTarget;                   // Total number of enemies to kill in this wave
        public float baseSpawnInterval;          // Base interval for spawning enemies
        public List<EnemyGroup> enemyGroups;     // Enemy group definitions
        [HideInInspector] public int killCount = 0;
    }

    [System.Serializable]
    public class EnemyGroup
    {
        public string enemyName;
        public int weight;                       // Spawn probability weight
        public int spawnAtOnce;                  // How many enemies spawn together
        public GameObject enemy;
    }

    [Header("Waves Setup")]
    public List<Wave> waves;
    public int currentWaveCount;
    public float waveInterval = 3f;
    private Transform player;
    private List<GameObject> activeEnemies = new List<GameObject>();

    [Header("Spawner Attributes")]
    private float spawnTimer;
    private float waveTime;                      // Time spent in the current wave

    [Header("Dynamic Difficulty")]
    public float difficultyRampRate = 0.003f;     // How quickly difficulty scales over time
    public float maxDifficultyMultiplier = 3f;   // Cap the difficulty multiplier

    bool IsInCameraView(Camera cam, Vector3 worldPosition)
    {
        Vector3 viewportPos = cam.WorldToViewportPoint(worldPosition);

        if (viewportPos.z < 0)
            return false;

        return viewportPos.x >= 0 && viewportPos.x <= 1 && viewportPos.y >= 0 && viewportPos.y <= 1;
    }


    void Start()
    {
        player = FindAnyObjectByType<PlayerController>().transform;
    }

    void Update()
    {
        if (currentWaveCount >= waves.Count) return;

        Wave currentWave = waves[currentWaveCount];

        // Check wave completion
        if (currentWave.killCount >= currentWave.killTarget)
        {
            StartCoroutine(StartNextWave());
            return;
        }

        // Track time spent in current wave
        waveTime += Time.deltaTime;
        spawnTimer += Time.deltaTime;

        // Calculate difficulty multiplier based on elapsed time
        float difficultyMultiplier = Mathf.Min(1f + waveTime * difficultyRampRate, maxDifficultyMultiplier);

        // Gradually reduce spawn interval (faster spawns)
        float adjustedSpawnInterval = currentWave.baseSpawnInterval / difficultyMultiplier;

        // Check if it's time to spawn
        if (spawnTimer >= adjustedSpawnInterval)
        {
            spawnTimer = 0f;
            SpawnEnemies(difficultyMultiplier);
        }
    }

    IEnumerator StartNextWave()
    {
        var remainingEnemies = new List<GameObject>(activeEnemies);

        Camera cam = Camera.main;

        foreach (GameObject enemy in remainingEnemies)
        {
            // Drop XP only for enemies visible in camera
            if (IsInCameraView(cam, enemy.transform.position))
            {
                var dropManager = enemy.GetComponent<DropRateManager>();
                if (dropManager != null)
                {
                    dropManager.DropItems();
                }
            }

            Destroy(enemy);
        }

        activeEnemies.Clear();

        // Short break between waves
        yield return new WaitForSeconds(waveInterval);

        // 🔁 Start next wave
        if (currentWaveCount < waves.Count - 1)
        {
            currentWaveCount++;
            waveTime = 0f; // reset wave timer
            spawnTimer = 0f;
        }
    }

    void SpawnEnemies(float difficultyMultiplier)
    {
        var wave = waves[currentWaveCount];
        if (wave.killCount >= wave.killTarget) return;

        // Weighted random enemy selection
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

        if (selectedGroup == null) return;

        // Scale spawn count dynamically
        int scaledSpawnCount = Mathf.RoundToInt(selectedGroup.spawnAtOnce * difficultyMultiplier);

        for (int i = 0; i < scaledSpawnCount; i++)
        {
            Vector2 spawnPosition = new Vector2(
                player.transform.position.x + Random.Range(-10f, 10f),
                player.transform.position.y + Random.Range(-10f, 10f)
            );

            GameObject enemy = Instantiate(selectedGroup.enemy, spawnPosition, Quaternion.identity);
            activeEnemies.Add(enemy);

            // 🔗 Assign wave and XP
            var enemyStats = enemy.GetComponent<EnemyStats>();
            if (enemyStats != null)
            {
                enemyStats.spawner = this;
                enemyStats.waveIndex = currentWaveCount;
            }

            var dropManager = enemy.GetComponent<DropRateManager>();
            if (dropManager != null)
            {
                dropManager.xpAmount = GetXPForWave(currentWaveCount);
            }
        }
    }

    // Called when enemy dies
    public void OnEnemyKilled(int waveIndex)
    {
        if (waveIndex < waves.Count)
        {
            waves[waveIndex].killCount++;
        }
        activeEnemies.RemoveAll(e => e == null);
    }

    int GetXPForWave(int wave)
    {
        switch (wave)
        {
            case 0: return 10;
            case 1: return 30;
            case 2: return 50;
            case 3: return 70;
            case 4: return 100;
            default: return 10;
        }
    }
}
