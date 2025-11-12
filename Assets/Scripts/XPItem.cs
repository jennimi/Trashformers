using UnityEngine;

public class XPItem : MonoBehaviour
{
    public int amount;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // add XP to player stats here
            Debug.Log($"Player gained {amount} XP!");
            Destroy(gameObject);
        }
    }
}
