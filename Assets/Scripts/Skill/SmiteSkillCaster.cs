using UnityEngine;
using System.Collections;

public class SmiteSkillCaster : MonoBehaviour
{
    public GameObject smitePrefab;
    public float cooldown = 5f;
    private bool canCast = true;

    public void CastRandomEnemy()
    {
        if (!canCast) return;

        // Use the new API
        EnemyStats[] enemies = Object.FindObjectsByType<EnemyStats>(FindObjectsSortMode.None);

        if (enemies.Length == 0) return;

        EnemyStats target = enemies[Random.Range(0, enemies.Length)];

        Instantiate(smitePrefab, target.transform.position, Quaternion.identity);

        StartCoroutine(CooldownRoutine());
    }

    private IEnumerator CooldownRoutine()
    {
        canCast = false;
        yield return new WaitForSeconds(cooldown);
        canCast = true;
    }
}
