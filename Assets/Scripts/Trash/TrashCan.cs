using UnityEngine;
using System.Collections;


public class TrashCan : MonoBehaviour
{
    public TrashType acceptedType;
    public WaveManager waveManager;
    public PlayerStorageUI ui;  // Drag in Inspector

    public float cooldownDuration = 1f;  // seconds
    private bool canReceiveTrash = true;

    private void OnTriggerStay2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        if (!canReceiveTrash) return;   // gate is closed

        PlayerStorage storage = other.GetComponent<PlayerStorage>();

        if (storage == null) return;
        if (storage.items.Count == 0) return;

        if (storage.items[0].type == acceptedType)
        {
            StartCoroutine(TrashCooldown());
            TrashType type = storage.RemoveFirstTrash();
            waveManager.ProgressWave();
        }
    }

    private IEnumerator TrashCooldown()
    {
        canReceiveTrash = false;
        Debug.Log("Trash can CLOSED: " + acceptedType);
        yield return new WaitForSeconds(cooldownDuration);
        canReceiveTrash = true;
        Debug.Log("Trash can OPENED: " + acceptedType);

    }

}
