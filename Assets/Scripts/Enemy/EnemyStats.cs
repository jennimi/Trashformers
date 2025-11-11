using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    public EnemyScriptableObject enemyData;

    // Current Stats
    float currentMoveSpeed;
    float currentHealth;
    float currentDamage;

    void Awake()
    {
        currentDamage = enemyData.Damage;
        currentMoveSpeed = enemyData.MoveSpeed;
        currentHealth = enemyData.MaxHealth;
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
        Destroy(gameObject);
    }

    // public void Update()
    // {
    //     if (Input.GetKeyDown(KeyCode.Space))
    //     {
    //         TakeDamage(10);
    //     }
    // }
}
