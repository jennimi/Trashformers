using UnityEngine;

public class SkillUpgradeManager : MonoBehaviour
{
    public static SkillUpgradeManager Instance;

    public SkillDefinition[] allSkills;      // assign in inspector
    public GameObject selectionUI;           // the popup UI
    public SkillOptionUI optionPrefab;       // button prefab for options
    public Transform optionParent;           // where buttons appear

    private PlayerSkillState playerState;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        playerState = FindAnyObjectByType<PlayerSkillState>();
        selectionUI.SetActive(false);
    }

    public void TriggerLevelUp()
    {
        Time.timeScale = 0f; // pause game
        selectionUI.SetActive(true);

        ShowRandomOptions();
    }

    void ShowRandomOptions()
    {
        // Remove old options
        foreach (Transform t in optionParent)
            Destroy(t.gameObject);

        // Pick 3 distinct random skills
        SkillDefinition[] copy = (SkillDefinition[])allSkills.Clone();
        System.Random rng = new System.Random();
        for (int i = 0; i < copy.Length; i++)
        {
            int rand = rng.Next(i, copy.Length);
            (copy[i], copy[rand]) = (copy[rand], copy[i]);
        }

        for (int i = 0; i < 3 && i < copy.Length; i++)
        {
            var option = Instantiate(optionPrefab, optionParent);
            option.Setup(copy[i]);
        }
    }

    public void SelectSkill(SkillDefinition skill)
    {
        playerState.LearnOrUpgrade(skill);

        UpgradeCasterFor(skill);

        // Re-enable gameplay
        selectionUI.SetActive(false);
        Time.timeScale = 1f;
    }

    private void UpgradeCasterFor(SkillDefinition skill)
    {
        var controller = FindAnyObjectByType<PlayerSkillController>();

        if (controller == null) return;

        if (skill == controller.bounceCaster.skillDefinition)
            controller.bounceCaster.LevelUp();

        if (skill == controller.incenseCaster.skillDefinition)
            controller.incenseCaster.LevelUp();

        if (skill == controller.smiteCaster.skillDefinition)
            controller.smiteCaster.LevelUp();
    }
}
