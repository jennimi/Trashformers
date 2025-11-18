using UnityEngine;

public class PlayerSkillEffectHandler : MonoBehaviour
{
    public PlayerSkillState state;

    public BouncingSkillCaster bouncing;
    public IncenseSkillCaster incense;
    public SmiteSkillCaster smite;

    void Update()
    {
        ApplyBouncingUpgrades();
        ApplyIncenseUpgrades();
        ApplySmiteUpgrades();
    }

    // ------------------------------
    // BOUNCING SKILL
    // ------------------------------
    private void ApplyBouncingUpgrades()
    {
        if (!state.HasSkill(bouncing.skillDefinition)) return;

        int lvl = state.GetLevel(bouncing.skillDefinition);

        bouncing.projectileCount = bouncing.baseProjectileCount + (lvl - 1) * bouncing.projectilesPerLevel;
    }

    // ------------------------------
    // INCENSE SKILL
    // ------------------------------
    private void ApplyIncenseUpgrades()
    {
        if (!state.HasSkill(incense.skillDefinition)) return;

        int lvl = state.GetLevel(incense.skillDefinition);

        incense.damagePerLevel = incense.baseDamage + (lvl - 1) * incense.damagePerLevel;
    }

    // ------------------------------
    // SMITE SKILL
    // ------------------------------
    private void ApplySmiteUpgrades()
    {
        if (!state.HasSkill(smite.skillDefinition)) return;

        int lvl = state.GetLevel(smite.skillDefinition);

        smite.cooldown = Mathf.Max(0.5f, smite.baseCooldown - (lvl - 1) * smite.cooldownReduction);
    }
}
