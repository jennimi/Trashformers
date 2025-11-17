using UnityEngine;

public class SmiteSkill : MonoBehaviour
{
    public float damage = 50f;       // Total damage
    public float radius = 2f;        // AoE size
    public float duration = 2f;    // How long the AoE stays
    private float timer = 0f;

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= duration)
            Destroy(gameObject);
    }

    private void FixedUpdate()
    {
        // Deal damage to all enemies in the AoE
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, radius);
        foreach (var col in hits)
        {
            if (col.TryGetComponent(out EnemyStats enemy))
            {
                enemy.TakeDamage(damage * Time.fixedDeltaTime); // DPS
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
