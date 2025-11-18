using UnityEngine;
using System.Collections;

public class SmiteSkillCaster : MonoBehaviour
{
    public GameObject smitePrefab;

    public SkillDefinition skillDefinition;

    [Header("Skill Stats")]
    public float baseDamage = 40f;
    public float damagePerLevel = 15f;

    public float baseRadius = 2f;
    public float radiusPerLevel = 0.3f;

    public float baseDuration = 1.5f;
    public float durationPerLevel = 0.2f;

    public int level = 1;

    public float baseCooldown = 5f;
    public float cooldownReduction = 0.25f;
    private bool canCast = true;

    [HideInInspector] public float damage;
    [HideInInspector] public float radius;
    [HideInInspector] public float cooldown;

    public void CastRandomEnemy()
    {
        if (!canCast) return;

        EnemyStats[] enemies = Object.FindObjectsByType<EnemyStats>(FindObjectsSortMode.None);
        if (enemies.Length == 0) return;

        EnemyStats target = enemies[Random.Range(0, enemies.Length)];

        GameObject smiteObj = Instantiate(smitePrefab, target.transform.position, Quaternion.identity);

        // Inject stats into the instantiated smite effect
        SmiteSkill smite = smiteObj.GetComponent<SmiteSkill>();
        smite.damage = GetDamage();
        smite.radius = GetRadius();
        smite.duration = GetDuration();

        StartCoroutine(CooldownRoutine());
    }

    private float GetDamage() => baseDamage + (level - 1) * damagePerLevel;
    private float GetRadius() => baseRadius + (level - 1) * radiusPerLevel;
    private float GetDuration() => baseDuration + (level - 1) * durationPerLevel;

    private IEnumerator CooldownRoutine()
    {
        canCast = false;
        yield return new WaitForSeconds(cooldown);
        canCast = true;
    }

    public void LevelUp()
    {
        level++;
    }
}
