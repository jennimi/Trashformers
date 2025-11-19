using UnityEngine;

public class IncenseSkill : MonoBehaviour
{
    [HideInInspector] public float damagePerSecond;

    private Transform player;
    private float tickRate = 0.2f;       
    private float tickTimer = 0f;

    private void Start()
    {
        player = FindAnyObjectByType<PlayerController>().transform;
    }

    private void Update()
    {
        // Follow player
        transform.position = player.position;

        tickTimer -= Time.deltaTime;
    }

    private void OnTriggerStay2D(Collider2D col)
    {
        if (tickTimer > 0f) return;       

        if (col.TryGetComponent(out EnemyStats enemy))
        {
            float damagePerTick = damagePerSecond * tickRate;
            enemy.TakeDamage(damagePerTick);
        }

        tickTimer = tickRate;             
    }
}
