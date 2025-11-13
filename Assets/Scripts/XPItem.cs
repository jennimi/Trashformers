using UnityEngine;

public class XPItem : MonoBehaviour
{
    public int amount;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }
}
