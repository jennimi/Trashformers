using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SkillOptionUI : MonoBehaviour
{
    public Image icon;
    public TMP_Text title;
    public TMP_Text levelText;
    public TMP_Text description;

    public SkillDefinition skill;
    public PlayerSkillState player;


    public void Setup(SkillDefinition s)
    {
        skill = s; // save reference for OnClick
        icon.sprite = s.icon;
        title.text = s.skillName;

        var player = FindAnyObjectByType<PlayerSkillState>();
        int lvl = player.GetLevel(s); // 0 if not learned

        levelText.text = $"Level {lvl + 1}";

        // Clamp lvl so we don't go out of bounds
        if (s.levelDescriptions != null && s.levelDescriptions.Count > 0)
        {
            int descIndex = Mathf.Clamp(lvl, 0, s.levelDescriptions.Count - 1);
            description.text = s.levelDescriptions[descIndex];
        }
        else
        {
            description.text = "";
        }
    }

    public void OnClick()
    {

        SkillUpgradeManager.Instance.SelectSkill(skill);
    }

}
