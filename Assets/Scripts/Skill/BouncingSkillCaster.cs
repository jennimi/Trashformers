using UnityEngine;
using System.Collections;

public class BouncingSkillCaster : MonoBehaviour
{
    public GameObject projectilePrefab;

    public SkillDefinition skillDefinition;

    [Header("Base Stats")]
    public int baseProjectileCount = 3;
    public int projectilesPerLevel = 1;

    public float baseDamage = 5f;
    public float damagePerLevel = 2f;

    public float baseCooldown = 4f;
    public float cooldownReduction = 0.25f;
    private bool canCast = true;

    public int level = 0;

    [HideInInspector] public int projectileCount;
    [HideInInspector] public float cooldown;

    public void Cast(Vector3 origin)
    {
        if (!canCast) return;

        int count = GetProjectileCount();
        float dmg = GetDamage();

        for (int i = 0; i < count; i++)
        {
            // Random angle 0–360 degrees
            float angle = Random.Range(0f, 360f);

            // Convert angle to direction
            Vector2 dir = Quaternion.Euler(0, 0, angle) * Vector2.right;

            GameObject projObj = Instantiate(projectilePrefab, origin, Quaternion.identity);

            BouncingSkill proj = projObj.GetComponent<BouncingSkill>();
            proj.Initialize(dir, dmg);
        }

        StartCoroutine(CooldownRoutine());
    }

    private float GetDamage() => baseDamage + (level - 1) * damagePerLevel;
    private int GetProjectileCount() => baseProjectileCount + (level - 1) * projectilesPerLevel;

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
