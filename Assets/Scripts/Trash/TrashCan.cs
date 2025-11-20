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
        if (!canReceiveTrash) return;

        // Cache components once
        PlayerStorage storage = other.GetComponent<PlayerStorage>();
        if (storage == null) return;
        if (storage.items.Count == 0) return;

        PlayerStats ps = other.GetComponent<PlayerStats>(); // Only need this once
        if (ps == null) return;

        TrashType firstItem = storage.RemoveFirstTrash();

        // Correct trash
        if (firstItem == acceptedType)
        {
            StartCoroutine(TrashCooldown());

            waveManager.AcceptTrash(acceptedType);
            return;
        }

        ps.StartCoroutine(ps.DebuffSpeed());
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
