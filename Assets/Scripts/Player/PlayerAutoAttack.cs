using UnityEngine;
using System.Collections;

public class PlayerAutoAttack : MonoBehaviour
{
    public GameObject projectilePrefab;
    public Transform firePoint;

    private PlayerStats stats;
    private PlayerController controller;

    private void Start()
    {
        stats = GetComponent<PlayerStats>();
        controller = GetComponent<PlayerController>();

        StartCoroutine(AutoAttackRoutine());
    }

    private IEnumerator AutoAttackRoutine()
    {
        while (true)
        {
            ShootProjectile();
            yield return new WaitForSeconds(stats.attackCooldown);
        }
    }

    private void ShootProjectile()
    {
        EnemyStats target = FindNearestEnemy();

        Vector2 shootDir;

        if (target != null)
            shootDir = (target.transform.position - transform.position).normalized;
        else
            shootDir = controller.lastMoveDir != Vector2.zero ? controller.lastMoveDir : Vector2.right;

        GameObject proj = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
        proj.GetComponent<Projectile>().Initialize(shootDir, stats.currentDamage);
    }

    private EnemyStats FindNearestEnemy()
    {
        float closestDist = Mathf.Infinity;
        EnemyStats closest = null;

        foreach (EnemyStats enemy in EnemyStats.AllEnemies)
        {
            if (enemy == null) continue;

            float dist = Vector2.Distance(transform.position, enemy.transform.position);
            if (dist < closestDist)
            {
                closestDist = dist;
                closest = enemy;
            }
        }

        return closest;
    }

}
