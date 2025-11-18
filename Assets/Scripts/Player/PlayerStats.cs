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

    public bool isInvincible = false;
    public float damageCooldown = 1f; // 1 second of invincibility

    public float attackCooldown = 3f;
    public bool canAttack = true;


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

        // initialize UI
        if (UIManager.Instance != null)
        {
            UIManager.Instance.UpdateHealth(currentHealth, playerData.MaxHealth);
            UIManager.Instance.UpdateEXP(experience, experienceCap);
            UIManager.Instance.UpdateLevel(level);
        }
    }

    public void IncreaseExperience(int amount)
    {
        experience += amount;
        if (UIManager.Instance != null)
            UIManager.Instance.UpdateEXP(experience, experienceCap);

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

            if (UIManager.Instance != null)
            {
                UIManager.Instance.UpdateLevel(level);
                UIManager.Instance.UpdateEXP(experience, experienceCap);
                UIManager.Instance.UpdateHealth(currentHealth, playerData.MaxHealth); // if HP changed
            }

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

    public void TakeDamage(float amount)
    {
        if (isInvincible)
            return; // ignore damage

        currentHealth -= amount;
        if (UIManager.Instance != null)
            UIManager.Instance.UpdateHealth(currentHealth, playerData.MaxHealth);
            
        Debug.Log($"Player took {amount} damage. HP = {currentHealth}");

        if (currentHealth <= 0)
        {
            Die();
            return;
        }

        StartCoroutine(DamageCooldownRoutine());
    }


    void Die()
    {
        Debug.Log("Player died!");
    }

    private IEnumerator DamageCooldownRoutine()
    {
        isInvincible = true;
        yield return new WaitForSeconds(damageCooldown);
        isInvincible = false;
    }

}
