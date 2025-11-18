using UnityEngine;

public class TrashInstance : MonoBehaviour
{

    public TrashType type;
    public TrashSpawner spawner;

    public GameObject prefab;


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        var storage = other.GetComponent<PlayerStorage>();

        // guard clause
        if (storage == null) return;
        
        if (storage.AddTrash(this))
        {
            spawner?.NotifyPickupCollected();

            Destroy(gameObject);
        }

    }
    
}
