using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Rigidbody2D rb;
    private Animator animator;

    private Vector2 movement;
    private Vector2 lastMoveDir;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // --- 1. Get raw input ---
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        // --- 2. Normalize for diagonal movement ---
        if (movement.sqrMagnitude > 1)
            movement.Normalize();

        // --- 3. Update animator parameters ---
        animator.SetFloat("MoveX", movement.x);
        animator.SetFloat("MoveY", movement.y);
        animator.SetFloat("Speed", movement.sqrMagnitude);

        // --- 4. Remember last direction for idle facing ---
        if (movement.sqrMagnitude > 0)
        {
            lastMoveDir = movement;
            animator.SetFloat("LastMoveX", lastMoveDir.x);
            animator.SetFloat("LastMoveY", lastMoveDir.y);
        }
    }

    void FixedUpdate()
    {
        // --- 5. Apply movement ---
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }
}
