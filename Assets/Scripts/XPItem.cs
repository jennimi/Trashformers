using UnityEngine;

public class XPItem : MonoBehaviour
{
    public int amount = 10;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerStats playerStats = collision.GetComponent<PlayerStats>();
            if (playerStats != null)
            {
                playerStats.IncreaseExperience(amount);
            }

            Destroy(gameObject);
        }
    }
}
