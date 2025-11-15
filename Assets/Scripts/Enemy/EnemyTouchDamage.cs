using UnityEngine;

public class EnemyTouchDamage : MonoBehaviour
{
    public float contactDamage = 10f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerStats stats = other.GetComponent<PlayerStats>();

            if (stats != null)
            {
                stats.TakeDamage(contactDamage);
            }
        }
    }
}
