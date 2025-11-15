using UnityEngine;

[CreateAssetMenu(fileName = "PlayerScriptableObject", menuName = "Scriptable Objects/PlayerScriptableObject")]
public class PlayerScriptableObject : ScriptableObject
{
     // Base Stats
    [SerializeField]
    float maxHealth;
    public float MaxHealth { get => maxHealth; private set => maxHealth = value; }
    
    [SerializeField]
    float damage;
    public float Damage { get => damage; private set => damage = value; }

    // Movement Stats
    [SerializeField] float baseMoveSpeed = 5f;
    public float BaseMoveSpeed => baseMoveSpeed;

    [SerializeField] float baseDashSpeed = 12f;
    public float BaseDashSpeed => baseDashSpeed;
}
