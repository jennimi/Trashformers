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


    public Button button;

    public Image iconImage;

    public void ResetVisual()
    {
        button.interactable = false;   // Forces state reset
        button.interactable = true;    // Forces Unity to redraw as normal
    }

    public void Setup(SkillDefinition s)
    {
        skill = s;
        icon.sprite = s.icon;

        // 🔥 Set the icon color here
        icon.color = s.iconColor;

        title.text = s.skillName;

        var player = FindAnyObjectByType<PlayerSkillState>();
        int lvl = player.GetLevel(s);

        levelText.text = $"Level {lvl + 1}";

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
        ResetVisual();
    }

}
