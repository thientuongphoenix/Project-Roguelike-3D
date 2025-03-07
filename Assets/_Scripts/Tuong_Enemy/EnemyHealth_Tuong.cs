using UnityEngine;

public class EnemyHealth_Tuong : MonoBehaviour
{
    public EnemyStats_Tuong enemyStats;

    private EnemyAnimationController_Tuong enemyAnim;

    void Start()
    {
        enemyStats.health = enemyStats.maxHealth;
        enemyAnim = GetComponent<EnemyAnimationController_Tuong>();
    }

    public void TakeDamage(int damage)
    {
        enemyStats.health -= damage;
        enemyAnim.ChangeAnimationState(EnemyAnimationState.Damage);

        if (enemyStats.health <= 0)
        {
            enemyStats.health = 0;
            Die();
        }
    }

    void Die()
    {
        Destroy(gameObject);
    }
}
