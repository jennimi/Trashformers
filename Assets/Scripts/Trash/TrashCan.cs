using UnityEngine;

public class TrashCan : MonoBehaviour
{
    public TrashType acceptedType;        
    public WaveManager waveManager; 
    public PlayerStorageUI ui;  // Drag in Inspector

    float deliverCooldown = 0.9f;
    float deliverTimer = 0f;

    private void Update()
    {
        if (deliverTimer > 0)
            deliverTimer -= Time.deltaTime;
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (deliverTimer > 0) return;
        if (!other.CompareTag("Player")) return;

        PlayerStorage storage = other.GetComponent<PlayerStorage>();

        if (storage == null) return;
        if (storage.items.Count == 0) return;

        if (storage.items[0].type == acceptedType)
        {
            TrashType type = storage.RemoveFirstTrash();

            waveManager.ProgressWave();

            deliverTimer = deliverCooldown;
        }
    }

}
