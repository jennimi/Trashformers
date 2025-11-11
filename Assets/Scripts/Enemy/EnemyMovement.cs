using UnityEngine;

public class EnemyMovement : MonoBehaviour
{

    Transform player;
    public float moveSpeed;

    void Start() {
        player = FindObjectOfType<PlayerController>().transform;
    }

    void Update() {
        // Constant move enemy to player
        transform.position = Vector2.MoveTowards(transform.position, player.transform.position, moveSpeed * Time.deltaTime);
    }
}
