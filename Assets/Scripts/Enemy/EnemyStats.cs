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
    [HideInInspector]    
    public EnemySpawner spawner;
    [HideInInspector]
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

        var dropManager = GetComponent<DropRateManager>();
        if (dropManager != null)
        {
            dropManager.DropItems();
        }

        Destroy(gameObject);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TakeDamage(10);
        }
    }
}
