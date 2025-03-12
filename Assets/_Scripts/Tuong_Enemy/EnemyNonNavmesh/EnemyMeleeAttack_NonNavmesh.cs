using System.Collections;
using UnityEngine;

public class EnemyMeleeAttack_NonNavmesh : MonoBehaviour
{
    public EnemyStats_Tuong enemyStats; // 🔥 Gán ScriptableObject cho Enemy
    public LayerMask playerLayer; // 🔥 Layer nhân vật để kiểm tra

    private Transform player;
    private EnemyAnimationController_Tuong enemyAnim;
    private float attackTimer = 0f; // 🔥 Bộ đếm thời gian cooldown

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        enemyAnim = GetComponent<EnemyAnimationController_Tuong>();
    }

    void Update()
    {
        if (player == null || !enemyStats.isMelee) return; // Không có Player hoặc không phải Enemy cận chiến

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // 🔥 Kiểm tra nếu đủ thời gian hồi chiêu & Player trong phạm vi tấn công
        if (distanceToPlayer <= enemyStats.attackRange && attackTimer <= 0f)
        {
            AttackPlayer();
        }
        else if (distanceToPlayer > enemyStats.attackRange) // Nếu Player ngoài phạm vi, chuyển sang trạng thái Move
        {
            enemyAnim.ChangeAnimationState(EnemyAnimationState.Move);
        }

        // 🔥 Giảm bộ đếm thời gian nếu còn cooldown
        attackTimer -= Time.deltaTime;
    }

    void AttackPlayer()
    {
        enemyAnim.ChangeAnimationState(EnemyAnimationState.Attack);

        // 🔥 Kiểm tra nếu Player vẫn trong phạm vi khi đòn đánh xảy ra
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, enemyStats.attackRange, playerLayer);
        foreach (Collider hit in hitColliders)
        {
            PlayerHealth playerHealth = hit.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(enemyStats.damage);
                //Debug.Log(gameObject.name + " gây " + enemyStats.damage + " sát thương lên Player!");
            }
        }

        // 🔥 Đặt lại bộ đếm thời gian để chờ cooldown tiếp theo
        attackTimer = enemyStats.attackCooldown;
    }

    void OnDrawGizmosSelected()
    {
        // 🔥 Vẽ hình cầu thể hiện phạm vi tấn công
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, enemyStats.attackRange);
    }
}
