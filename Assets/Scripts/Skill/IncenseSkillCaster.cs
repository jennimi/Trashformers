using UnityEngine;
using System.Collections;

public class IncenseSkillCaster : MonoBehaviour
{
    public GameObject aoePrefab;

    public SkillDefinition skillDefinition;

    [Header("Skill Stats")]
    public float baseDamage = 5f;
    public float damagePerLevel = 2f;
    public int level = 0;

    [HideInInspector] public float damagePerSecond;

    private GameObject activeAura;

    public void Cast()
    {
        if (activeAura != null) return; // Already active

        activeAura = Instantiate(aoePrefab, transform.position, Quaternion.identity);
        IncenseSkill incense = activeAura.GetComponent<IncenseSkill>();
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
