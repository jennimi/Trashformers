using UnityEngine;
using System.Collections.Generic;

public class SkillUIManager : MonoBehaviour
{
    public SkillDefinition[] allSkills;     // Assign all skills here (SO assets)
    public SkillOptionUI[] optionSlots;     // Assign UI slots (3 buttons/cards)
    public GameObject panel;
    private PlayerSkillState player;

    void Start()
    {
        player = FindAnyObjectByType<PlayerSkillState>();
        ShowRandomSkillOptions();
    }

    public void ShowRandomSkillOptions()
    {
        if (player == null)
            player = FindAnyObjectByType<PlayerSkillState>();

        // 1. Filter skills that are NOT max level
        List<SkillDefinition> eligible = new List<SkillDefinition>();

        foreach (var s in allSkills)
        {
            int lvl = player.GetLevel(s);
            if (lvl < s.maxLevel)
                eligible.Add(s);
        }

        // 2. If all skills are maxed → do nothing
        if (eligible.Count == 0)
        {
            Debug.Log("All skills maxed out — no more upgrades.");
            foreach (var slot in optionSlots)
                slot.gameObject.SetActive(false);
            return;
        }

        // 3. Shuffle
        Shuffle(eligible);

        // 4. Fill UI slots
        for (int i = 0; i < optionSlots.Length; i++)
        {
            if (i < eligible.Count)
            {
                optionSlots[i].gameObject.SetActive(true);
                optionSlots[i].player = player;
                optionSlots[i].Setup(eligible[i]);
            }
            else
            {
                optionSlots[i].gameObject.SetActive(false);
            }
        }
    }

    // Overload for list shuffling
    private void Shuffle(List<SkillDefinition> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int rand = Random.Range(i, list.Count);
            (list[i], list[rand]) = (list[rand], list[i]);
        }
    }

    public void OpenSkillUI()
    {
        // Check if anything can still level up
        bool hasUpgradeable = false;

        foreach (var s in allSkills)
        {
            if (player.GetLevel(s) < s.maxLevel)
            {
                hasUpgradeable = true;
                break;
            }
        }

        if (!hasUpgradeable)
        {
            // No upgrade → skip opening the panel
            Debug.Log("All skills maxed — skipping skill UI.");
            return;
        }

        // Otherwise, open normally
        panel.SetActive(true);
        ShowRandomSkillOptions();
        Time.timeScale = 0f;
    }

}