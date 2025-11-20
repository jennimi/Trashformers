using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerStats : MonoBehaviour
{
    public PlayerScriptableObject playerData;

    // RUNTIME stats (modifiable)
    public float currentHealth;
    public float maxHealth;
    public float currentDamage;
    public float currentMoveSpeed;
    public float currentDashSpeed;

    public bool isDead = false;

    [Header("Experience / Level")]
    public int experience = 0;
    public int level = 1;
    public int experienceCap;

    [Header("Audio")]
    public AudioClip expDingClip;
    private AudioSource audioSource;


    [System.Serializable]
    public class LevelRange
    {
        public int startLevel;
        public int endLevel;
        public int experienceCapIncrease;
    }

    public List<LevelRange> levelRanges;

    [Header("Per Level Stat Increase")]
    public float healthIncreasePerLevel = 25f;
    public float damageIncreasePerLevel = 5f;
    public float MoveSpeedIncreasePerLevel = 0.5f;
    public float DashSpeedIncreasePerLevel = 0.5f;

    [Header("Damage Cooldown")]
    public bool isInvincible = false;
    public float damageCooldown = 1f;

    [Header("Attack Cooldown")]
    public float attackCooldown = 3f;
    public bool canAttack = true;

    void Awake()
    {
        // Copy from ScriptableObject ONCE
        maxHealth = playerData.MaxHealth;
        currentHealth = maxHealth;

        currentDamage = playerData.Damage;
        currentDashSpeed = playerData.BaseDashSpeed;
        currentMoveSpeed = playerData.BaseMoveSpeed;

        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
    }

    void Update()
    {
        // DEBUG: Press G to instantly kill the player
        if (Input.GetKeyDown(KeyCode.G))
        {
            Debug.Log("DEBUG: Killing player with G key");
            currentHealth = 0;
            TakeDamage(0);  // forces death logic
        }
    }


    void Start()
    {
        // Initial EXP cap (using first range)
        experienceCap = levelRanges[0].experienceCapIncrease;

        if (UIManager.Instance != null)
        {
            UIManager.Instance.UpdateHealth(currentHealth, maxHealth);
            UIManager.Instance.UpdateEXP(experience, experienceCap);
            UIManager.Instance.UpdateLevel(level);
        }
    }

    // ---------------- EXPERIENCE ----------------
    public void IncreaseExperience(int amount)
    {
        experience += amount;

        if (expDingClip != null)
            audioSource.PlayOneShot(expDingClip);

        UIManager.Instance.UpdateEXP(experience, experienceCap);

        LevelUpChecker();
    }

    void LevelUpChecker()
    {
        while (experience >= experienceCap)
        {
            level++;
            experience -= experienceCap;

            // Get experience cap increase for this bracket
            int increase = 0;
            foreach (LevelRange range in levelRanges)
            {
                if (level >= range.startLevel && level <= range.endLevel)
                {
                    increase = range.experienceCapIncrease;
                    break;
                }
            }

            experienceCap += increase;

            // Upgrade player stats
            IncreaseStatsOnLevelUp();

            if (UIManager.Instance != null)
            {
                UIManager.Instance.UpdateLevel(level);
                UIManager.Instance.UpdateEXP(experience, experienceCap);
                UIManager.Instance.UpdateHealth(currentHealth, playerData.MaxHealth); // if HP changed
            }

            
            FindAnyObjectByType<SkillUIManager>().OpenSkillUI();
        }
    }

    // ---------------- LEVEL UP STAT INCREASE ----------------
    public void IncreaseStatsOnLevelUp()
    {
        // Increase Max Health (runtime)
        maxHealth += healthIncreasePerLevel;

        // Heal player to new max
        currentHealth = maxHealth;

        // Increase other stats
        currentDamage += damageIncreasePerLevel;

        if (currentDashSpeed < 25)
            currentDashSpeed += DashSpeedIncreasePerLevel;

        if (currentMoveSpeed < 20)
            currentMoveSpeed += MoveSpeedIncreasePerLevel;

        Debug.Log($"LEVEL UP → HP: {currentHealth}/{maxHealth}, DMG: {currentDamage}");
    }

    // ---------------- DAMAGE ----------------
    public void TakeDamage(float amount)
    {
        if (isInvincible) return;

        currentHealth -= amount;
        if (currentHealth < 0) currentHealth = 0;

        UIManager.Instance.UpdateHealth(currentHealth, maxHealth);

        if (currentHealth <= 0)
        {
            Die();
            return;
        }

        StartCoroutine(DamageCooldownRoutine());
    }

    IEnumerator DamageCooldownRoutine()
    {
        isInvincible = true;
        yield return new WaitForSeconds(damageCooldown);
        isInvincible = false;
    }

    void Die()
    {
        if (isDead) return;

        isDead = true;
        Debug.Log("Player died!");

        // stop player input
        GetComponent<PlayerController>().enabled = false;

        // play animation once
        Animator anim = GetComponent<Animator>();
        if (anim != null)
            anim.SetBool("Dead", true);

        // trigger Game Over
        GameManager.Instance.TriggerGameOver();
    }

}