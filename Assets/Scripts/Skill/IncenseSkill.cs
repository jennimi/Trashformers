using UnityEngine;

public class IncenseSkill : MonoBehaviour
{
    [HideInInspector] public float damagePerSecond;

    private Transform player;

    private void Start()
    {
        player = FindAnyObjectByType<PlayerController>().transform;
    }

    private void Update()
    {
        // Follow player
        transform.position = player.position;
    }

    private void OnTriggerStay2D(Collider2D col)
    {
        if (col.TryGetComponent(out EnemyStats enemy))
        {
            enemy.TakeDamage(damagePerSecond * Time.deltaTime);
        }
    }
}
