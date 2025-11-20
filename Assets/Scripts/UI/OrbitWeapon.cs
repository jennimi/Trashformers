using UnityEngine;

public class OrbitWeapon : MonoBehaviour
{
    public Transform center;      // Player
    public float radius = 1.5f;
    public float orbitSpeed = 180f;
    public float enemyDetectRange = 5f;

    private float angle;
    private bool orbiting = true;

    void Update()
    {
        if (center == null) return;

        EnemyStats target = FindNearestEnemy();

        if (target != null && Vector2.Distance(center.position, target.transform.position) <= enemyDetectRange)
        {
            // Stop orbiting and point toward enemy
            orbiting = false;
            Vector2 dirToEnemy = (target.transform.position - center.position).normalized;
            transform.position = center.position + (Vector3)(dirToEnemy * radius);

            // Optional: face enemy
            float zRot = Mathf.Atan2(dirToEnemy.y, dirToEnemy.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, zRot);
        }
        else
        {
            // Orbit around player
            orbiting = true;
            angle += orbitSpeed * Time.deltaTime;
            float rad = angle * Mathf.Deg2Rad;
            Vector3 offset = new Vector3(Mathf.Cos(rad), Mathf.Sin(rad), 0) * radius;
            transform.position = center.position + offset;

            transform.rotation = Quaternion.Euler(0, 0, angle); // optional: tangent rotation
        }
    }

    private EnemyStats FindNearestEnemy()
    {
        float closestDist = Mathf.Infinity;
        EnemyStats closest = null;

        foreach (EnemyStats enemy in EnemyStats.AllEnemies)
        {
            if (enemy == null) continue;

            float dist = Vector2.Distance(center.position, enemy.transform.position);
            if (dist < closestDist)
            {
                closestDist = dist;
                closest = enemy;
            }
        }

        return closest;
    }
}