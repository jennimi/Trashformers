using UnityEngine;

public class PlayerSkillController : MonoBehaviour
{
    public PlayerSkillState playerState;

    public BouncingSkillCaster bounceCaster;
    public IncenseSkillCaster incenseCaster;
    public SmiteSkillCaster smiteCaster;

    void Update()
    {
        HandleBouncingSkill();
        HandleIncenseSkill();
        HandleSmiteSkill();
    }

    private void HandleBouncingSkill()
    {
        if (playerState.HasSkill(bounceCaster.skillDefinition))
        {
            bounceCaster.Cast(transform.position);
        }
    }

    private void HandleIncenseSkill()
    {
        if (playerState.HasSkill(incenseCaster.skillDefinition))
        {
            incenseCaster.Cast();
        }
    }

    private void HandleSmiteSkill()
    {
        if (playerState.HasSkill(smiteCaster.skillDefinition))
        {
            smiteCaster.CastRandomEnemy();
        }
    }
}

