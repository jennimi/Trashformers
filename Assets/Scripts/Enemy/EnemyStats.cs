using UnityEngine;
using System.Collections;

public class EnemyStats : MonoBehaviour
{
    public EnemyScriptableObject enemyData;

    // Current Stats
    [HideInInspector]
    public float currentMoveSpeed;
    [HideInInspector]
    public float currentHealth;
    [HideInInspector]
    public float currentDamage;
    public EnemySpawner spawner;
    public int waveIndex;
    Transform player;

    void Awake()
    {
        currentDamage = enemyData.Damage;
        currentMoveSpeed = enemyData.MoveSpeed;
        currentHealth = enemyData.MaxHealth;
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

        Destroy(gameObject);
    }

    public void Update()
    {
        if (player == null) return;

        // Debug damage trigger
        if (Input.GetKeyDown(KeyCode.Space))
        {
            EnemyStats enemy = GetComponent<EnemyStats>();
            enemy.TakeDamage(5);
        }
    }
}
