using UnityEngine;

public class BouncingSkill : MonoBehaviour
{
    public float speed = 10f;
    public float damage = 5f;

    public float lifetime = 5f;    // ← projectile exists for 5 seconds
    private float timer = 0f;

    private Vector2 direction;

    private Vector2 minBound;
    private Vector2 maxBound;

    public void Initialize(Vector2 dir, float dmg)
    {
        direction = dir.normalized;
        damage = dmg;
    }

    void Update()
    {
        CameraBounds.Instance.UpdateBounds();
        var min = CameraBounds.Instance.Min;
        var max = CameraBounds.Instance.Max;

        Move();
        StayInsideCamera(min, max);
        // Lifetime countdown
        timer += Time.deltaTime;
        if (timer >= lifetime)
            Destroy(gameObject);
    }

    void StayInsideCamera(Vector2 min, Vector2 max)
    {
        Vector2 pos = transform.position;

        float edgeBuffer = 0.1f;

        // Bounce Left/Right
        if (pos.x < min.x + edgeBuffer)
        {
            pos.x = min.x + edgeBuffer;
            direction.x *= -1;
        }
        else if (pos.x > max.x - edgeBuffer)
        {
            pos.x = max.x - edgeBuffer;
            direction.x *= -1;
        }

        // Bounce Top/Bottom
        if (pos.y < min.y + edgeBuffer)
        {
            pos.y = min.y + edgeBuffer;
            direction.y *= -1;
        }
        else if (pos.y > max.y - edgeBuffer)
        {
            pos.y = max.y - edgeBuffer;
            direction.y *= -1;
        }

        transform.position = pos;
    }

    void Move()
    {
        transform.Translate(direction * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.TryGetComponent(out EnemyStats enemy))
        {
            enemy.TakeDamage(damage);

            // Bounce back when hitting enemy
            direction = -direction;
        }
    }
}
