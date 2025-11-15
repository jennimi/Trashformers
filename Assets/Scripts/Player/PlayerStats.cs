using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerStats : MonoBehaviour
{
    public PlayerScriptableObject playerData;
    public float currentHealth;
    public float currentDamage;
    public float currentMoveSpeed;
    public float currentDashSpeed;

    [Header("Experience/Level")]
    public int experience = 0;
    public int level = 1;
    public int experienceCap;

    [System.Serializable]
    public class LevelRange
    {
        public int startLevel;
        public int endLevel;
        public int experienceCapIncrease;

    }

    public float healthIncreasePerLevel = 25f;
    public float damageIncreasePerLevel = 5f;
    public float MoveSpeedIncreasePerLevel = 0.5f;
    public float DashSpeedIncreasePerLevel = 0.5f;

    public List<LevelRange> levelRanges;

    void Awake()
    {
        currentHealth = playerData.MaxHealth;
        currentDamage = playerData.Damage;
        currentDashSpeed = playerData.BaseDashSpeed;
        currentMoveSpeed = playerData.BaseMoveSpeed;
    }

    void Start()
    {
        experienceCap = levelRanges[0].experienceCapIncrease;
    }

    public void IncreaseExperience(int amount)
    {
        experience += amount;

        LevelUpChecker();
    }

    void LevelUpChecker()
    {
        if (experience >= experienceCap)
        {
            Debug.Log("Player is leveling up");
            level++;
            experience -= experienceCap;

            // Increase experience cap for next level
            int experienceCapIncrease = 0;
            foreach (LevelRange range in levelRanges)
            {
                experienceCapIncrease = range.experienceCapIncrease;
                break;
            }
            experienceCap += experienceCapIncrease;

            // 🔥 Increase stats here
            IncreaseStatsOnLevelUp();
        }
    }

    void IncreaseStatsOnLevelUp()
    {
        currentHealth += healthIncreasePerLevel;
        currentDamage += damageIncreasePerLevel;
        if (currentDashSpeed < 25)
        {
            currentDashSpeed += DashSpeedIncreasePerLevel;
        }

        if (currentMoveSpeed < 20)
        {
            currentMoveSpeed += MoveSpeedIncreasePerLevel;
        }

        Debug.Log($"New Stats → HP: {currentHealth}, DMG: {currentDamage}");
    }
}
