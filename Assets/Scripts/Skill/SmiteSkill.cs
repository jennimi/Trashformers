using UnityEngine;

public class SmiteSkill : MonoBehaviour
{
    [HideInInspector] public float damage; 
    [HideInInspector] public float radius;
    [HideInInspector] public float duration;

    private float timer = 0f;

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= duration)
            Destroy(gameObject);
    }

    void FixedUpdate()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, radius);
        foreach (var col in hits)
        {
            if (col.TryGetComponent(out EnemyStats enemy))
            {
                enemy.TakeDamage(damage * Time.fixedDeltaTime);
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
