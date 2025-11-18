using UnityEngine;
using System.Collections;

public class IncenseSkillCaster : MonoBehaviour
{
    public GameObject aoePrefab;

    public SkillDefinition skillDefinition;

    [Header("Skill Stats")]
    public float baseDamage = 5f;
    public float damagePerLevel = 2f;
    public int level = 1;

    private bool canCast = true;

    [HideInInspector] public float damagePerSecond;

    public void Cast()
    {
        if (!canCast) return;

        GameObject incenseObj = Instantiate(aoePrefab, transform.position, Quaternion.identity);

        // Pass level-scaled stats to the incense effect
        IncenseSkill incense = incenseObj.GetComponent<IncenseSkill>();
        incense.damagePerSecond = GetDamage();
    }

    public float GetDamage()
    {
        return baseDamage + (level - 1) * damagePerLevel;
    }

    // Call this when player upgrades the skill
    public void LevelUp()
    {
        level++;
    }
}
