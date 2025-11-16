using UnityEngine;

public class TrashSpawner : MonoBehaviour
{

    public Transform player;
    public GameObject trashPrefab;   // prefab of TrashPickup
    public TrashType[] possibleTrashes;

    public float spawnInterval = 2f; // seconds
    public float radius = 6f;
    public int maxActive = 10;

    float timer;
    int activeCount = 0;


    // Update is called once per frame
    void Update()
    {
        if (player == null || trashPrefab == null || possibleTrashes == null || possibleTrashes.Length == 0) return;

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
        if (activeCount >= 5) return;

        Vector2 offset = Random.insideUnitCircle * radius;
        Vector3 spawnPos = player.position + (Vector3)offset;
        TrashType type = possibleTrashes[Random.Range(0, possibleTrashes.Length)];

        var go = Instantiate(type.prefab, spawnPos, Quaternion.identity);

        var pickup = go.GetComponent<TrashPickup>();
        pickup.spawner = this;
        pickup.trashType = type;

        activeCount++;
        // Reduce count when destroyed
        Destroy(go, 60f); // auto-despawn in 60s to avoid clutter
    }

    public void NotifyPickupCollected()
    {
        activeCount = Mathf.Max(0, activeCount - 1);
        Debug.Log("Active Count: " + activeCount);
    }
}
