using System.Collections;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.AI;

public class EnemyHealth_Tuong : MonoBehaviour
{
    public EnemyStats_Tuong enemyStats;

    private EnemyAnimationController_Tuong enemyAnim;

    public GameObject dropItemPrefab; // Prefab vật phẩm rớt ra khi chết

    private bool isDead = false;

    void Start()
    {
        enemyStats.health = enemyStats.maxHealth;
        enemyAnim = GetComponent<EnemyAnimationController_Tuong>();
        isDead = false;
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
        isDead = true;
        enemyAnim.ChangeAnimationState(EnemyAnimationState.Die); // Phát animation chết

        // Dừng NavMeshAgent hoặc các hành động khác (nếu có)
        NavMeshAgent agent = GetComponent<NavMeshAgent>();
        if (agent != null)
        {
            agent.isStopped = true;
            agent.enabled = false;
        }

        // Chờ cho animation chết hoàn thành
        float deathAnimationLength = 1f; // Điều chỉnh thời gian theo animation thực tế
        yield return new WaitForSeconds(deathAnimationLength);

        // Rớt vật phẩm sau khi chết
        if (dropItemPrefab != null && isDead)
        {
            Instantiate(dropItemPrefab, transform.position, Quaternion.identity);
        }

        // Chờ thêm 2 giây rồi xoá Enemy
        yield return new WaitForSeconds(0f);

        // Trả Enemy về Pool thay vì Destroy
        EnemyPool_Tuong.Instance.ReturnEnemy(gameObject);
        //Destroy(gameObject);
    }
}
