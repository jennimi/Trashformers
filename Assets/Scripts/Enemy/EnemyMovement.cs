using UnityEngine;

public class EnemyMovement : MonoBehaviour {
    public EnemyScriptableObject enemyData;
    Transform player;

    void Start()
    {
        player = FindFirstObjectByType<PlayerController>().transform;
    }

    void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, player.transform.position, enemyData.MoveSpeed * Time.deltaTime);
    }
}
