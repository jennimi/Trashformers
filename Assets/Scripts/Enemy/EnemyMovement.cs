using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public EnemyScriptableObject enemyData;

    private Transform player;
    private Animator anim;

    void Start()
    {
        player = FindFirstObjectByType<PlayerController>().transform;
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (player == null) return;

        // Calculate direction to player
        Vector2 direction = (player.position - transform.position).normalized;

        // Update Animator parameters
        anim.SetFloat("PlayerX", direction.x);
        anim.SetFloat("PlayerY", direction.y);

        // Move toward player
        transform.position = Vector2.MoveTowards(
            transform.position,
            player.position,
            enemyData.MoveSpeed * Time.deltaTime
        );
    }
}
