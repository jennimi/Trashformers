using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class TrashSpawner : MonoBehaviour
{

    public Transform player;
    public WaveManager waveManager; // use the list from here for allowed types to spawn

    // spawning relevant variables
    public float spawnInterval = 2f; // seconds
    public float radius = 6f;
    public int maxActive = 10;
    int activeCount = 0;

    float timer;


    void Awake()
    {
        waveManager = FindObjectOfType<WaveManager>();
        Debug.Log("WaveManager found: " + waveManager);
    }

    // Update is called once per frame
    void Update()
    {
        if (player == null || waveManager == null || waveManager.allowedTypes.Count == 0) return;

        timer += Time.deltaTime;
        if (timer >= spawnInterval && activeCount < maxActive)
        {
            SpawnRandomTrash();
            print("Spawned Trash | Active Count: " + activeCount);
            timer = 0f;
        }
    }

    void SpawnRandomTrash()
    {
        if (activeCount >= maxActive) return;

        var possibleTrashes = waveManager.allowedTypes
            .Where(t => {
                waveManager.recycleCounts.TryGetValue(t, out int current);
                return current < waveManager.requiredAmountToRecyclePerType;
            })
            .ToList();

        if (possibleTrashes.Count == 0) return;

        TrashType type = possibleTrashes[Random.Range(0, possibleTrashes.Count)];

        // choose prefab from category
        if (type.prefabs == null || type.prefabs.Count == 0) return;
        GameObject prefab = type.prefabs[Random.Range(0, type.prefabs.Count)];

        // position for spawning
        Vector2 offset = Random.insideUnitCircle * radius;
        Vector3 spawnPos = player.position + (Vector3)offset;


        var go = Instantiate(prefab, spawnPos, Quaternion.identity);

        var instance = go.GetComponent<TrashInstance>();
        instance.spawner = this;
        instance.type = type;
        instance.prefab = prefab;

        activeCount++;
    }

    public void NotifyPickupCollected()
    {
        activeCount = Mathf.Max(0, activeCount - 1);
        Debug.Log("Active Count: " + activeCount);
    }
}
