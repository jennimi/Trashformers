using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;
    private Rigidbody2D rb;
    private Animator animator;
    private Vector2 movement;
    private Vector2 lastMoveDir;

    [Header("Dash Settings")]
    public float dashSpeed = 12f;
    public float dashDuration = 0.2f;
    public float dashCooldown = 1f;
    private bool isDashing = false;
    private bool canDash = true;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (isDashing) return; // skip movement input while dashing

        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        if (movement.sqrMagnitude > 1)
            movement.Normalize();

        animator.SetFloat("MoveX", movement.x);
        animator.SetFloat("MoveY", movement.y);
        animator.SetFloat("Speed", movement.sqrMagnitude);

        if (movement.sqrMagnitude > 0.01f)
        {
            lastMoveDir = new Vector2(
                Mathf.RoundToInt(Mathf.Clamp(movement.x, -1f, 1f)),
                Mathf.RoundToInt(Mathf.Clamp(movement.y, -1f, 1f))
            );

            animator.SetFloat("LastMoveX", lastMoveDir.x);
            animator.SetFloat("LastMoveY", lastMoveDir.y);
        }

        // --- Dash input ---
        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash)
        {
            StartCoroutine(Dash());
        }
    }

    void FixedUpdate()
    {
        if (isDashing) return;
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }

    private IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;

        // ✅ Tell Animator we're dashing
        animator.SetBool("isDashing", true);

        Vector2 dashDirection = (movement.sqrMagnitude > 0.01f) ? movement : lastMoveDir;
        float startTime = Time.time;

        while (Time.time < startTime + dashDuration)
        {
            rb.MovePosition(rb.position + dashDirection.normalized * dashSpeed * Time.fixedDeltaTime);
            yield return null;
        }

        isDashing = false;
        animator.SetBool("isDashing", false);  // ✅ Back to Idle/Walk

        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }

}
