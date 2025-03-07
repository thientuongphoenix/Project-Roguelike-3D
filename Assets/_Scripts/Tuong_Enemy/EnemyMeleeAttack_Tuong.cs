using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMeleeAttack_Tuong : MonoBehaviour
{
    public EnemyStats_Tuong enemyStats; // Gán ScriptableObject chứa thông tin Enemy
    public Transform attackPoint; // Điểm trung tâm vùng tấn công
    public LayerMask playerLayer; // Layer của Player

    private bool canAttack = true;
    private NavMeshAgent agent;
    private Transform player;

    private EnemyAnimationController_Tuong enemyAnim;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player")?.transform;

        enemyAnim = GetComponent<EnemyAnimationController_Tuong>();
    }

    void Update()
    {
        if (player != null)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);

            // Nếu trong phạm vi tấn công và có thể đánh
            if (distanceToPlayer <= enemyStats.attackRange && canAttack)
            {
                StartCoroutine(AttackPlayer());
            }
            else
            {
                agent.SetDestination(player.position); // Tiếp tục di chuyển đến Player
            }
        }
    }

    IEnumerator AttackPlayer()
    {
        canAttack = false;
        agent.isStopped = true; // 🛑 Dừng di chuyển để đảm bảo animation Attack không bị cắt
        enemyAnim.ChangeAnimationState(EnemyAnimationState.Attack);

        // Chờ animation Attack hoàn tất trước khi gây sát thương
        float attackAnimationLength = 1.0f; // ⏳ Thời gian animation
        yield return new WaitForSeconds(attackAnimationLength * 0.5f); // 💭 Đợi nửa thời gian trước khi gây sát thương

        // Kiểm tra xem Player có trong tầm đánh không
        Collider[] hitPlayers = Physics.OverlapSphere(attackPoint.position, enemyStats.attackRange, playerLayer);
        foreach (Collider playerCollider in hitPlayers)
        {
            PlayerHealth playerHealth = playerCollider.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(enemyStats.damage);
                Debug.Log("Gây " + enemyStats.damage + " sát thương lên Player");
            }
        }

        // Đợi animation hoàn thành trước khi chuyển sang trạng thái khác
        yield return new WaitForSeconds(attackAnimationLength * 0.5f);

        agent.isStopped = false; // ✅ Bật lại di chuyển
        canAttack = true;
    }

    void OnDrawGizmosSelected()
    {
        if (attackPoint != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(attackPoint.position, enemyStats.attackRange);
        }
    }
}
