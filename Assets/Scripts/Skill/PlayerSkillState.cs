using System.Collections.Generic;
using UnityEngine;

public class PlayerSkillState : MonoBehaviour
{
    public Dictionary<SkillDefinition, int> learnedSkills = new();

    public bool HasSkill(SkillDefinition skill)
        => learnedSkills.ContainsKey(skill);

    public int GetLevel(SkillDefinition skill)
        => learnedSkills.TryGetValue(skill, out var lvl) ? lvl : 0;

    public void LearnOrUpgrade(SkillDefinition skill)
    {
        if (!learnedSkills.ContainsKey(skill))
            learnedSkills[skill] = 1;
        else
            learnedSkills[skill]++;
    }
}
