using UnityEngine;

public class EnemyHealth_Tuong : MonoBehaviour
{
    public EnemyStats_Tuong enemyStats;

    void Start()
    {
        enemyStats.health = enemyStats.maxHealth;
    }

    public void TakeDamage(int damage)
    {
        enemyStats.health -= damage;

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
