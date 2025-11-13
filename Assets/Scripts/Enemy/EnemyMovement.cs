using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public EnemyScriptableObject enemyData;
    private Transform player;

    void Start()
    {
        player = FindFirstObjectByType<PlayerController>().transform;
    }

    void Update()
    {
        if (player == null) return;

        transform.position = Vector2.MoveTowards(
            transform.position,
            player.position,
            enemyData.MoveSpeed * Time.deltaTime
        );
    }
}
