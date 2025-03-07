using UnityEngine;

[CreateAssetMenu(fileName = "EnemyStats", menuName = "Enemy/EnemyStats")]
public class EnemyStats_Tuong : ScriptableObject
{
    [Header("Attack Type")]
    public bool isRanged; // Enemy đánh xa
    public bool isMelee;  // Enemy đánh gần

    [Header("Health")]
    public int health;      // Máu hiện tại
    public int maxHealth;   // Máu tối đa

    [Header("Combat Stats")]
    public int attackRange; // Tầm đánh
    public int damage;      // Sát thương
    public float attackCooldown; // Delay đòn đánh
}
