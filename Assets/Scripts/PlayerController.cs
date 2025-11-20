using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    private PlayerStats stats;
    private Rigidbody2D rb;
    private Animator animator;

    private Vector2 movement;
    public Vector2 lastMoveDir { get; private set; }

    [Header("Dash Settings")]
    public float dashDuration = 0.3f;
    public float dashAnimBuffer = 0.15f;
    public float dashCooldown = 60f;

    private bool isDashing = false;
    private bool canDash = true;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        stats = GetComponent<PlayerStats>();
    }

    void Update()
    {
        if (isDashing) return;

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
                Mathf.RoundToInt(Mathf.Clamp(movement.x, -1, 1)),
                Mathf.RoundToInt(Mathf.Clamp(movement.y, -1, 1))
            );

            animator.SetFloat("LastMoveX", lastMoveDir.x);
            animator.SetFloat("LastMoveY", lastMoveDir.y);
        }

        // DASH INPUT
        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash)
        {
            StartCoroutine(Dash());
        }

        // PAUSE
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GameManager.Instance.TogglePause();
        }

    }

    void FixedUpdate()
    {
        if (isDashing) return;
        rb.MovePosition(rb.position + movement * stats.currentMoveSpeed * Time.fixedDeltaTime);
    }

    private IEnumerator Dash()
    {
        canDash = false;

        // 🔥 Start cooldown UI through UIManager
        if (UIManager.Instance != null)
            UIManager.Instance.StartDashCooldown(dashCooldown);

        isDashing = true;
        animator.SetBool("isDashing", true);

        Vector2 dashDirection = (movement.sqrMagnitude > 0.01f) ? movement : lastMoveDir;
        float startTime = Time.time;

        // physical dash burst
        while (Time.time < startTime + dashDuration)
        {
            rb.MovePosition(rb.position + dashDirection.normalized * stats.currentDashSpeed * Time.fixedDeltaTime);
            yield return new WaitForFixedUpdate();
        }

        // keep animation alive slightly longer
        yield return new WaitForSeconds(dashAnimBuffer);

        isDashing = false;
        animator.SetBool("isDashing", false);

        // ⏳ WAIT FOR THE SAME COOLDOWN UI IS USING
        yield return new WaitForSeconds(dashCooldown);

        canDash = true;
    }
}