using UnityEngine;

public class EnemyMovement : MonoBehaviour {
    public EnemyScriptableObject enemyData;
    
    private Rigidbody2D rb;
    private Transform player;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.useFullKinematicContacts = true; // prevents push


        player = FindFirstObjectByType<PlayerController>().transform;
    }

    void FixedUpdate()
    {
        Vector2 newPosition = Vector2.MoveTowards(
            rb.position,
            player.position,
            enemyData.MoveSpeed * Time.fixedDeltaTime
        );

        rb.MovePosition(newPosition);
    }
}

