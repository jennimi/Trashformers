using UnityEngine;

public class PlayerSkillController : MonoBehaviour
{
    public BouncingSkillCaster skillCaster;  // must be the component, not GameObject
    public float skillCooldown = 2f;

    private float nextCastTime;

    void Start()
    {
        nextCastTime = Time.time + skillCooldown;
    }

    void Update()
    {
        if (Time.time >= nextCastTime)
        {
            nextCastTime = Time.time + skillCooldown;
            skillCaster.Cast(transform.position);
        }
    }
}
