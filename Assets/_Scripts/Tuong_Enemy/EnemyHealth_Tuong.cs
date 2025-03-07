using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyHealth_Tuong : MonoBehaviour
{
    public EnemyStats_Tuong enemyStats;

    private EnemyAnimationController_Tuong enemyAnim;

    public GameObject dropItemPrefab; // Prefab vật phẩm rớt ra khi chết

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
            StartCoroutine(Die());
        }
    }

    IEnumerator Die()
    {
        enemyAnim.ChangeAnimationState(EnemyAnimationState.Die); // Phát animation chết

        // 🛑 Dừng NavMeshAgent hoặc các hành động khác (nếu có)
        NavMeshAgent agent = GetComponent<NavMeshAgent>();
        if (agent != null)
        {
            agent.isStopped = true;
            agent.enabled = false;
        }

        // 🎯 Chờ cho animation chết hoàn thành
        float deathAnimationLength = 1f; // ⏳ Điều chỉnh thời gian theo animation thực tế
        yield return new WaitForSeconds(deathAnimationLength);

        // 💰 Rớt vật phẩm sau khi chết
        if (dropItemPrefab != null)
        {
            Instantiate(dropItemPrefab, transform.position, Quaternion.identity);
        }

        // ⏳ Chờ thêm 2 giây rồi xoá Enemy
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
    }
}
