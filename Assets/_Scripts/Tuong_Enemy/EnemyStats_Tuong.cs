using UnityEngine;

[CreateAssetMenu(fileName = "EnemyStats", menuName = "Enemy/EnemyStats")]
public class EnemyStats_Tuong : ScriptableObject
{
    [Header("Attack Type")]
    public bool isRanged; // Enemy đánh xa
    public bool isMelee;  // Enemy đánh gần

    [Header("Health")]
    public float health;      // Máu hiện tại
    public float maxHealth;   // Máu tối đa

    [Header("Combat Stats")]
    public float attackRange; // Tầm đánh
    public float damage;      // Sát thương
    public float attackCooldown; // Delay đòn đánh

    [Header("Reward")]
    public float expReward; // EXP thưởng khi chết
}
