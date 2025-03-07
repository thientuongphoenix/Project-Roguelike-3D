using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMeleeAttack_Tuong : MonoBehaviour
{
    public EnemyStats_Tuong enemyStats; // Gán ScriptableObject ch?a thông tin Enemy
    public Transform attackPoint; // ?i?m trung tâm vùng t?n công
    public LayerMask playerLayer; // Layer c?a Player

    private bool canAttack = true;
    private NavMeshAgent agent;
    private Transform player;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    void Update()
    {
        if (player != null)
        {
            agent.SetDestination(player.position);

            float distanceToPlayer = Vector3.Distance(transform.position, player.position);
            if (distanceToPlayer <= enemyStats.attackRange && canAttack)
            {
                StartCoroutine(AttackPlayer());
            }
        }
    }

    IEnumerator AttackPlayer()
    {
        canAttack = false;
        Debug.Log(gameObject.name + " t?n công Player!");

        // Ki?m tra xem Player có trong t?m ?ánh không
        Collider[] hitPlayers = Physics.OverlapSphere(attackPoint.position, enemyStats.attackRange, playerLayer);
        foreach (Collider playerCollider in hitPlayers)
        {
            PlayerHealth playerHealth = playerCollider.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(enemyStats.damage);
                Debug.Log("Gây " + enemyStats.damage + " sát th??ng lên Player");
            }
        }

        yield return new WaitForSeconds(enemyStats.attackCooldown);
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
