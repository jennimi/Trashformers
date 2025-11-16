using UnityEngine;

public class TrashPickup : MonoBehaviour
{

    public TrashType trashType;
    public TrashSpawner spawner;


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        var storage = other.GetComponent<PlayerStorage>();
        if (storage != null)
        {
            if (storage.AddTrash(trashType))
            {
                spawner?.NotifyPickupCollected();
                FindObjectOfType<PlayerStorageController>()?.RefreshUI();


                Destroy(gameObject);
            }

        }

    }
    
}
