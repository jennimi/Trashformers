using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyStats : MonoBehaviour
{
    public EnemyScriptableObject enemyData;

    public static List<EnemyStats> AllEnemies = new List<EnemyStats>();

    void OnEnable()
    {
        AllEnemies.Add(this);
    }

    void OnDisable()
    {
        AllEnemies.Remove(this);
    }

    // Current Stats
    [HideInInspector]
    public float currentMoveSpeed;
    [HideInInspector]
    public float currentHealth;
    [HideInInspector]
    public float currentDamage;
    [HideInInspector]
    public EnemySpawner spawner;
    [HideInInspector]
    public int waveIndex;
    Transform player;

    void Awake()
    {
        PlayerStats player = FindAnyObjectByType<PlayerStats>();

        int level = player.level;

        currentDamage = enemyData.Damage * (1 + (level - 1) * 0.1f);    // +10% per level
        currentMoveSpeed = enemyData.MoveSpeed;
        currentHealth = enemyData.MaxHealth * (1 + (level - 1) * 0.2f); // +20% per level
    }

    void Start()
    {
        player = FindAnyObjectByType<PlayerController>().transform;
    }

    public void TakeDamage(float dmg)
    {
        currentHealth -= dmg;

        if (currentHealth <= 0)
        {
            Kill();
        }
    }

    public void Kill()
    {
        if (spawner != null)
        {
            spawner.OnEnemyKilled(waveIndex);
        }

        var dropManager = GetComponent<DropRateManager>();
        if (dropManager != null)
        {
            dropManager.DropItems();
        }

        Destroy(gameObject);
    }
}
