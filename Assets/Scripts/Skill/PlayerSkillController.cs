using UnityEngine;

public class PlayerSkillController : MonoBehaviour
{
    public BouncingSkillCaster bounceCaster;
    public IncenseSkillCaster incenseCaster;
    public SmiteSkillCaster smiteCaster;

    void Update()
    {
        // Auto-cast bouncing every time cooldown allows
        // bounceCaster.Cast(transform.position);

        // Auto-cast AOE incense
        // incenseCaster.Cast();

        // Auto-cast AOE Smite
        smiteCaster.CastRandomEnemy();
    }
}
