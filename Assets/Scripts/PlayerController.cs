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
    public float dashDuration = 0.3f;     // shorter movement burst
    public float dashAnimBuffer = 0.15f;  // keep dash animation alive briefly after movement
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
        if (isDashing) return; // skip normal input during dash

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
        animator.SetBool("isDashing", true);

        Vector2 dashDirection = (movement.sqrMagnitude > 0.01f) ? movement : lastMoveDir;
        float startTime = Time.time;

        // actual movement burst
        while (Time.time < startTime + dashDuration)
        {
            rb.MovePosition(rb.position + dashDirection.normalized * dashSpeed * Time.fixedDeltaTime);
            yield return new WaitForFixedUpdate(); // use physics tick
        }

        // stop physical dash, but keep dash animation alive a bit longer
        yield return new WaitForSeconds(dashAnimBuffer);

        isDashing = false;
        animator.SetBool("isDashing", false);

        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }
}
