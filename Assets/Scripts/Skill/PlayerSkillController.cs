using UnityEngine;

public class PlayerSkillController : MonoBehaviour
{
    public BouncingSkillCaster bounceCaster;
    public IncenseSkillCaster incenseCaster;

    void Update()
    {
        // Auto-cast bouncing every time cooldown allows
        // bounceCaster.Cast(transform.position);

        // Auto-cast AOE incense
        incenseCaster.Cast();
    }
}
