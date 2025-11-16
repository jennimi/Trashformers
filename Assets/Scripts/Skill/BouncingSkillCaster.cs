using UnityEngine;

public class BouncingSkillCaster : MonoBehaviour
{
    public GameObject projectilePrefab;
    public int projectileCount = 3;
    public float damage = 5f;

    public void Cast(Vector3 origin)
    {
        for (int i = 0; i < projectileCount; i++)
        {
            // Random angle 0–360 degrees
            float angle = Random.Range(0f, 360f);

            // Convert angle to direction
            Vector2 dir = Quaternion.Euler(0, 0, angle) * Vector2.right;

            // Spawn projectile
            GameObject projObj = Instantiate(projectilePrefab, origin, Quaternion.identity);

            // Initialize bouncing projectile
            BouncingSkill proj = projObj.GetComponent<BouncingSkill>();
            proj.Initialize(dir, damage);
        }
    }

}
