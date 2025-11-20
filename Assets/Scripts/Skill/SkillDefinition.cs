using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "Skill", menuName = "ScriptableObjects/Skill")]
public class SkillDefinition : ScriptableObject
{
    public string skillName;
    public Sprite icon;

    public int maxLevel = 3;

    public Color iconColor = Color.white;

    public List<string> levelDescriptions = new List<string>();
}
