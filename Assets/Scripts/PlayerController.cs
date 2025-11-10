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
        // --- 1. Get input ---
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        // --- 2. Normalize for diagonal movement ---
        if (movement.sqrMagnitude > 1)
            movement.Normalize();

        // --- 3. Update movement parameters ---
        animator.SetFloat("MoveX", movement.x);
        animator.SetFloat("MoveY", movement.y);
        animator.SetFloat("Speed", movement.sqrMagnitude);

        // --- 4. Record last move direction ---
        if (movement.sqrMagnitude > 0.01f)
        {
            // Snap to nearest cardinal direction (-1, 0, 1)
            lastMoveDir = new Vector2(
                Mathf.RoundToInt(Mathf.Clamp(movement.x, -1f, 1f)),
                Mathf.RoundToInt(Mathf.Clamp(movement.y, -1f, 1f))
            );

            animator.SetFloat("LastMoveX", lastMoveDir.x);
            animator.SetFloat("LastMoveY", lastMoveDir.y);
        }
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }
}
