using UnityEngine;

public class IncenseSkill : MonoBehaviour
{
    public float damagePerSecond = 5f;
    public float duration = 5f;

    private Transform player;
    private float timer;

    private void Start()
    {
        player = FindAnyObjectByType<PlayerController>().transform;
    }

    private void Update()
    {
        // Follow the player
        transform.position = player.position;

        // Lifetime
        timer += Time.deltaTime;
        if (timer >= duration)
            Destroy(gameObject);
    }

    private void OnTriggerStay2D(Collider2D col)
    {
        if (col.TryGetComponent(out EnemyStats enemy))
        {
            enemy.TakeDamage(damagePerSecond * Time.deltaTime);
        }
    }
}
